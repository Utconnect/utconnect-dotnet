using IdentityProvider.Domain.Models;
using Shared.Authentication.Models;

namespace Shared.Authentication.Services;

public interface IJwtService
{
    GeneratedToken CreateToken(User user);
}