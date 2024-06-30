using System.Security.Claims;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;

namespace Oidc.Presentation.Controllers;

public class AuthorizationController(IOpenIddictApplicationManager applicationManager) : Controller
{
    [HttpPost("~/token")]
    public async Task<IActionResult> Exchange()
    {
        OpenIddictRequest request = HttpContext.GetOpenIddictServerRequest() ??
            throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

        if (request.IsRefreshTokenGrantType())
        {
            AuthenticateResult authResult =
                await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            ClaimsPrincipal? claimsPrincipal = authResult.Principal;

            if (claimsPrincipal == null)
            {
                throw new InvalidOperationException("The claims principal cannot be retrieved.");
            }

            return SignIn(claimsPrincipal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        if (request.ClientId == null)
        {
            throw new InvalidOperationException("The application cannot be found.");
        }

        object application = await applicationManager.FindByClientIdAsync(request.ClientId) ??
            throw new InvalidOperationException("The application cannot be found.");

        // Create a new ClaimsIdentity containing the claims that
        // will be used to create an id_token, a token or a code.
        // ClaimsIdentity identity = new(TokenValidationParameters.DefaultAuthenticationType,
        //     OpenIddictConstants.Claims.Name, OpenIddictConstants.Claims.Role);

        // Use the client_id as the subject identifier.
        // identity.SetClaim(OpenIddictConstants.Claims.Subject, await applicationManager.GetClientIdAsync(application));
        // identity.SetClaim(OpenIddictConstants.Claims.Name, await applicationManager.GetDisplayNameAsync(application));
        //
        // identity.SetDestinations(static claim => claim.Type switch
        // {
        //     // Allow the "name" claim to be stored in both the access and identity tokens
        //     // when the "profile" scope was granted (by calling principal.SetScopes(...)).
        //     OpenIddictConstants.Claims.Name when
        //         claim.Subject != null && claim.Subject.HasScope(OpenIddictConstants.Permissions.Scopes.Profile)
        //         => [OpenIddictConstants.Destinations.AccessToken, OpenIddictConstants.Destinations.IdentityToken],
        //
        //     // Otherwise, only store the claim in the access tokens.
        //     _ => [OpenIddictConstants.Destinations.AccessToken]
        // });

        var clientId = request.ClientId;
        var identity = new ClaimsIdentity(authenticationType: TokenValidationParameters.DefaultAuthenticationType);

        identity.SetClaim(OpenIddictConstants.Claims.Subject, clientId);
        identity.SetScopes(request.GetScopes());
        var principal = new ClaimsPrincipal(identity);
        // Returning a SignInResult will ask OpenIddict to issue the appropriate access/identity tokens.
        return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

        // return SignIn(new ClaimsPrincipal(identity), OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }
}