using Oidc.Domain.Models;

namespace IdentityProvider.Application.Services.Abstract;

public interface IOidcService
{
    Task<ExchangeTokenResponse?> Exchange(CancellationToken cancellationToken);
}