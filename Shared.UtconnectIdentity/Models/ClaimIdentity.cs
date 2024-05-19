namespace Shared.UtconnectIdentity.Models;

public class ClaimIdentity(ITenant tenant, IUser user) : IIdentity
{
    public ITenant Tenant { get; } = tenant;
    public IUser User { get; } = user;
}