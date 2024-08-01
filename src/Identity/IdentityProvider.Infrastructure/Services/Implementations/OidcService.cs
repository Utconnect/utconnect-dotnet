using IdentityProvider.Domain.Models;
using IdentityProvider.Infrastructure.Services.Abstract;
using Microsoft.Extensions.Options;
using Oidc.Domain.Models;
using RestSharp;
using RestSharp.Authenticators;
using Shared.Application.Configuration.Models;
using Shared.Authentication.Models;
using Shared.Authentication.Services;

namespace IdentityProvider.Infrastructure.Services.Implementations;

public class OidcService(IJwtService jwtService, IOptions<OidcConfig> oidcConfig) : IOidcService
{
    public async Task<ExchangeTokenResponse?> Exchange(User user, CancellationToken cancellationToken)
    {
        GeneratedToken jwtToken = jwtService.CreateToken(user);
        JwtAuthenticator authenticator = new(jwtToken.Token);
        RestClientOptions options = new(oidcConfig.Value.Url)
        {
            Authenticator = authenticator
        };
        RestClient client = new(options);

        RestRequest request = new(oidcConfig.Value.TokenPath);
        request.AddParameter("scope", "test_scope offline_access username email")
            .AddParameter("grant_type", "client_credentials")
            .AddParameter("client_id", "test_client")
            .AddParameter("client_secret", "test_secret");

        RestResponse<ExchangeTokenResponse> response =
            await client.ExecutePostAsync<ExchangeTokenResponse>(request, cancellationToken);
        return response.Data;
    }
}