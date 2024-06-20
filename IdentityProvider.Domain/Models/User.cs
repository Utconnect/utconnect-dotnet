using Microsoft.AspNetCore.Identity;
using Shared.UtconnectIdentity.Models;

namespace IdentityProvider.Domain.Models;

public sealed class User : IdentityUser<Guid>, IUser
{
    public User()
    {
    }

    public User(string userName, string name) : base(userName)
    {
        Name = name;
        Id = Guid.NewGuid();
    }

    public string Name { get; set; } = default!;
    public bool IsAuthenticated { get; set; }
    public List<int> Permissions { get; } = [];
    public List<int> Roles { get; } = [];
}