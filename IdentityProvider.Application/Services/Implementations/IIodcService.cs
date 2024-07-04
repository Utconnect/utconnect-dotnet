using IdentityProvider.Application.Services.Abstract;
using Microsoft.Extensions.Options;
using Oidc.Domain.Models;
using RestSharp;
using Shared.Application.Configuration;

namespace IdentityProvider.Application.Services.Implementations;

public class OidcService(IOptions<OidcConfig> oidcConfig) : IOidcService
{
    public async Task<ExchangeTokenResponse?> Exchange(CancellationToken cancellationToken)
    {
        RestClientOptions options = new(oidcConfig.Value.Url);
        RestClient client = new(options);

        RestRequest request = new("/token");
        request.AddParameter("scope", "test_scope offline_access")
            .AddParameter("grant_type", "client_credentials")
            .AddParameter("client_id", "test_client")
            .AddParameter("client_secret", "test_secret");

        RestResponse<ExchangeTokenResponse> timeline =
            await client.ExecutePostAsync<ExchangeTokenResponse>(request, cancellationToken);
        return timeline.Data;
    }
}