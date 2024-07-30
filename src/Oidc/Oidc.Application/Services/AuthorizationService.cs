using System.Security.Claims;
using OpenIddict.Abstractions;

namespace Oidc.Application.Services;

public static class AuthorizationService
{
    private const string AccessToken = OpenIddictConstants.Destinations.AccessToken;
    private const string IdentityToken = OpenIddictConstants.Destinations.IdentityToken;

    public static List<string> GetDestinations(ClaimsIdentity identity, Claim claim)
    {
        return (claim.Type, identity.HasScope(OpenIddictConstants.Scopes.OpenId)) switch
        {
            (OpenIddictConstants.Claims.Username, _) => [AccessToken, IdentityToken],
            (OpenIddictConstants.Claims.Email, true) => [IdentityToken],
            _ => []
        };
    }
}