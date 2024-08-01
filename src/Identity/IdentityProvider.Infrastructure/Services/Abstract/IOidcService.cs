using IdentityProvider.Domain.Models;
using Oidc.Domain.Models;

namespace IdentityProvider.Infrastructure.Services.Abstract;

public interface IOidcService
{
    Task<ExchangeTokenResponse?> Exchange(User user, CancellationToken cancellationToken);
}