using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Oidc.Domain.Models;
using RestSharp;
using RestSharp.Authenticators;
using Shared.Application.Configuration.Models;

namespace Home.Application.User.Queries.GetUserInfo;

public record GetUserInfoQuery(string AccessToken) : IRequest<UserInfoResponse?>;

public class GetUserInfoQueryHandler(IOptions<OidcConfig> oidcConfig, ILogger<GetUserInfoQueryHandler> logger)
    : IRequestHandler<GetUserInfoQuery, UserInfoResponse?>
{
    public async Task<UserInfoResponse?> Handle(GetUserInfoQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting user information from IdentityProvider");

        JwtAuthenticator authenticator = new(query.AccessToken);
        RestClientOptions options = new(oidcConfig.Value.Url)
        {
            Authenticator = authenticator
        };
        RestClient client = new(options);

        RestRequest request = new(oidcConfig.Value.UserInfoPath);

        RestResponse<UserInfoResponse> response =
            await client.ExecuteGetAsync<UserInfoResponse>(request, cancellationToken);

        return response.Data;
    }
}