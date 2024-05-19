using Microsoft.AspNetCore.Identity;
using Shared.UtconnectIdentity.Models;

namespace IdentityProvider.Domain.Models;

// Add profile data for application users by adding properties to the IdentityProviderUser class
public class User : IdentityUser<Guid>, IUser
{
    public User()
    {
    }

    public User(string userName, string name) : base(userName)
    {
        Name = name;
    }

    public Guid Identifier { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = default!;
    public bool IsAuthenticated { get; set; }
    public List<int> Permissions { get; } = [];
    public List<int> Roles { get; } = [];
}