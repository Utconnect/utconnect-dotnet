using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Shared.Application.Extensions;
using Shared.Logging;
using Shared.UtconnectIdentity.Exceptions;
using Shared.UtconnectIdentity.Models;

namespace Shared.UtconnectIdentity.Services;

public class IdentityService(IHttpContextAccessor httpContextAccessor, ILogger<IdentityService> logger)
    : IIdentityService
{
    private readonly ClaimsPrincipal? _user = httpContextAccessor.HttpContext?.User;

    private static ITenant DefaultTenant => new ClaimTenant(
        new Guid("3350b37e-4433-412b-adb9-df40e2478525"),
        "ID",
        "Utconnect Administration",
        1
    );

    public IIdentity? GetCurrent()
    {
        if (_user == null)
        {
            return null;
        }

        IUser user;
        try
        {
            user = MapCurrentUser(_user.Claims);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            throw;
        }

        return new ClaimIdentity(DefaultTenant, user);
    }

    private static ClaimUser MapCurrentUser(IEnumerable<Claim> claims)
    {
        List<Claim> claimsList = claims.ToList();
        if (claims == null || claimsList.Count == 0)
        {
            throw new NoClaimException();
        }

        var identifier = claimsList.Find(x => x.Type == ClaimTypes.NameIdentifier)?.Value ??
                         Guid.Empty.ToString();
        var userName = claimsList.Find(x => x.Type == ClaimTypes.Name)?.Value ?? string.Empty;
        var name = claimsList.Find(x => x.Type == ClaimTypes.GivenName)?.Value ?? string.Empty;

        var permission = claimsList.Find(x => x.Type == "permissions");
        List<int> permissions = permission == null
            ? []
            : JsonConvert.DeserializeObject<List<int>>(permission.Value) ?? [];

        var loginDate = (claimsList.Find(x => x.Type == "nbf")?.Value ?? string.Empty).ToUnixDateTime()
                        ?? throw new InvalidClaimUnixDateTimeException();
        var expirationDate =
            (claimsList.Find(x => x.Type == ClaimTypes.Expiration)?.Value ?? string.Empty).ToUnixDateTime()
            ?? throw new InvalidClaimUnixDateTimeException();

        var roleIdClaims = claimsList.Find(x => x.Type == "role_ids");
        List<int> roles = roleIdClaims == null
            ? []
            : JsonConvert.DeserializeObject<List<int>>(roleIdClaims.Value) ?? [];

        return new ClaimUser(Guid.Parse(identifier), userName, name, IsAuthenticated(loginDate, expirationDate),
            permissions, roles);
    }

    public (string AuthenticationScheme, ClaimsPrincipal, AuthenticationProperties authProperties) GetNewClaims(
        IUser user)
    {
        var now = DateTimeOffset.UtcNow;
        var expiration = now.AddHours(6);

        List<Claim> claims =
        [
            new Claim(ClaimTypes.NameIdentifier, user.Identifier.ToString()),
            new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
            new Claim(ClaimTypes.GivenName, user.Name),
            new Claim(ClaimTypes.Role, "sysadmin"),
            new Claim("nbf", now.ToUnixTimeSeconds().ToString()),
            new Claim(ClaimTypes.Expiration, expiration.ToUnixTimeSeconds().ToString())
        ];

        var claimsIdentity = new ClaimsIdentity(
            claims, CookieAuthenticationDefaults.AuthenticationScheme);

        var authProperties = new AuthenticationProperties
        {
            ExpiresUtc = expiration,
            IssuedUtc = now
        };

        return (
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);
    }

    private static bool IsAuthenticated(DateTime loginTime, DateTime expirationDate)
    {
        var currentTime = DateTime.Now;
        return loginTime < currentTime && currentTime < expirationDate;
    }
}