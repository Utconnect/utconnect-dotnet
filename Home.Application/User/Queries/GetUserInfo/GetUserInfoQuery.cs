using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Oidc.Domain.Models;
using RestSharp;
using RestSharp.Authenticators;
using Shared.Application.Configuration.Models;

namespace Home.Application.User.Queries.GetUserInfo;

public record GetUserInfoQuery(string AccessToken) : IRequest<int?>;

public class GetUserInfoQueryHandler(IOptions<OidcConfig> oidcConfig, ILogger<GetUserInfoQueryHandler> logger)
    : IRequestHandler<GetUserInfoQuery, int?>
{
    public async Task<int?> Handle(GetUserInfoQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting user information from IdentityProvider");

        JwtAuthenticator authenticator = new(query.AccessToken);
        RestClientOptions options = new(oidcConfig.Value.Url)
        {
            Authenticator = authenticator
        };
        RestClient client = new(options);

        RestRequest request = new(oidcConfig.Value.UserInfoPath);

        RestResponse<ExchangeTokenResponse> response =
            await client.ExecutePostAsync<ExchangeTokenResponse>(request, cancellationToken);

        return 0;
    }
}