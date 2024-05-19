using Shared.UtconnectIdentity.Models;

namespace IdentityProvider.Domain.Models;

public class Tenant(Guid identifier, string code, string name, int key)
    : ITenant
{
    public Guid Identifier { get; } = identifier;
    public string Code { get; } = code;
    public string Name { get; } = name;
    public int Key { get; } = key;
}