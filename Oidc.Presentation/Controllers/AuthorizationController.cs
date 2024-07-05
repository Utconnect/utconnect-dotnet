using System.Security.Claims;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;

namespace Oidc.Presentation.Controllers;

public class AuthorizationController(IOpenIddictApplicationManager applicationManager) : Controller
{
    [HttpPost("~/token")]
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

        ClaimsIdentity identity = new(authenticationType: TokenValidationParameters.DefaultAuthenticationType);
        identity.SetScopes(request.GetScopes());
        // Application claims
        identity.SetClaim(OpenIddictConstants.Claims.ClientId, await applicationManager.GetClientIdAsync(application));
        identity.SetClaim(OpenIddictConstants.Claims.Name, await applicationManager.GetDisplayNameAsync(application));
        // User claims
        identity.SetClaim(OpenIddictConstants.Claims.Subject, userId);
        identity.SetClaim(OpenIddictConstants.Claims.Username, userName);
        identity.SetClaim(OpenIddictConstants.Claims.Email, userEmail);

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