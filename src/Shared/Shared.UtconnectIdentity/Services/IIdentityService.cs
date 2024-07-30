using Shared.UtconnectIdentity.Models;

namespace Shared.UtconnectIdentity.Services;

public interface IIdentityService
{
    IIdentity? GetCurrent();
}