using Shared.UtconnectIdentity.Models;

namespace IdentityProvider.Domain.Models;

public class Identity(ITenant tenant, IUser user) : IIdentity
{
    public ITenant Tenant { get; } = tenant;
    public IUser User { get; } = user;
}