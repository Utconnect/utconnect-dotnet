using System.Security.Claims;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Oidc.Application.Services;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;

namespace Oidc.Presentation.Controllers;

[Route("connect")]
public class AuthorizationController(IOpenIddictApplicationManager applicationManager) : Controller
{
    [HttpPost("token")]
    public async Task<IActionResult> Exchange()
    {
        OpenIddictRequest request = HttpContext.GetOpenIddictServerRequest()
            ?? throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

        if (request.IsClientCredentialsGrantType())
        {
            return await HandleClientCredentialsGrantType(request);
        }

        if (request.IsRefreshTokenGrantType())
        {
            return await HandleRefreshTokenGrantType();
        }

        throw new InvalidOperationException("The specified grant type is not supported.");
    }

    [Authorize(AuthenticationSchemes = OpenIddictServerAspNetCoreDefaults.AuthenticationScheme)]
    [HttpGet("user-info"), HttpPost("user-info")]
    public async Task<IActionResult> Userinfo()
    {
        ClaimsPrincipal? claimsPrincipal =
            (await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme)).Principal;
        if (claimsPrincipal == null)
        {
            return Forbid(
                authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                properties: new AuthenticationProperties(new Dictionary<string, string?>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidGrant,
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                        "Cannot find user from the token"
                }));
        }

        Dictionary<string, object> claims = new(StringComparer.Ordinal)
        {
            [OpenIddictConstants.Claims.Subject] =
                claimsPrincipal.GetClaim(OpenIddictConstants.Claims.Subject) ?? string.Empty,
            [OpenIddictConstants.Claims.Issuer] =
                claimsPrincipal.GetClaim(OpenIddictConstants.Claims.Issuer) ?? string.Empty
        };

        Dictionary<string, string> scopeClaims = new()
        {
            { OpenIddictConstants.Permissions.Scopes.Email, OpenIddictConstants.Claims.Email },
            { OpenIddictConstants.Claims.Username, OpenIddictConstants.Claims.Username }
        };

        foreach ((string scope, string claim) in scopeClaims)
        {
            if (User.HasScope(scope))
            {
                claims[claim] = claimsPrincipal.GetClaim(claim) ?? string.Empty;
            }
        }

        return Ok(claims);
    }

    private async Task<IActionResult> HandleClientCredentialsGrantType(OpenIddictRequest request)
    {
        AuthenticateResult authResult = await HttpContext.AuthenticateAsync(OpenIddictConstants.TokenTypes.Bearer);
        ClaimsPrincipal? claimsPrincipal = authResult.Principal;
        if (claimsPrincipal == null)
        {
            throw new InvalidOperationException("The claims principal cannot be retrieved.");
        }

        if (request.ClientId == null)
        {
            throw new InvalidOperationException("The application cannot be found.");
        }

        string? userId = claimsPrincipal.GetClaim(ClaimTypes.NameIdentifier);
        string? userName = claimsPrincipal.GetClaim(JwtRegisteredClaimNames.Name);
        string? userEmail = claimsPrincipal.GetClaim(ClaimTypes.Email);

        if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(userName))
        {
            return Forbid(
                authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                properties: new AuthenticationProperties(new Dictionary<string, string?>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidGrant,
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                        "Cannot find user from the token"
                }));
        }

        object application = await applicationManager.FindByClientIdAsync(request.ClientId) ??
            throw new InvalidOperationException("The application cannot be found");

        ClaimsIdentity identity = new(TokenValidationParameters.DefaultAuthenticationType,
            OpenIddictConstants.Claims.Name, OpenIddictConstants.Claims.Role);

        identity.SetScopes(request.GetScopes());
        identity.SetClaim(OpenIddictConstants.Claims.ClientId, await applicationManager.GetClientIdAsync(application))
            .SetClaim(OpenIddictConstants.Claims.Subject, userId)
            .SetClaim(OpenIddictConstants.Claims.Username, userName)
            .SetClaim(OpenIddictConstants.Claims.Email, userEmail);

        identity.SetDestinations(c => AuthorizationService.GetDestinations(identity, c));

        ClaimsPrincipal principal = new(identity);

        return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }

    private async Task<IActionResult> HandleRefreshTokenGrantType()
    {
        AuthenticateResult authResult =
            await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

        ClaimsPrincipal? claimsPrincipal = authResult.Principal;
        if (claimsPrincipal == null)
        {
            throw new InvalidOperationException("The claims principal cannot be retrieved.");
        }

        string? userId = claimsPrincipal.GetClaim(OpenIddictConstants.Claims.Subject);
        if (string.IsNullOrEmpty(userId))
        {
            return Forbid(
                authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                properties: new AuthenticationProperties(new Dictionary<string, string?>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidGrant,
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                        "Cannot find user from the token."
                }));
        }

        return SignIn(claimsPrincipal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }
}