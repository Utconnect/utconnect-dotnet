using Microsoft.AspNetCore.Identity;

namespace IdentityProvider.Domain.Models;

public class Role : IdentityRole<Guid>
{
    public Role()
    {
    }

    public Role(string roleName) : base(roleName)
    {
    }
}