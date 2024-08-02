using IdentityProvider.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IdentityProvider.Infrastructure.Persistence;

public class IdentityProviderContextInitializer(
    ILogger<IdentityProviderContextInitializer> logger,
    IdentityProviderContext context,
    UserManager<User> userManager,
    RoleManager<Role> roleManager)
{
    public async Task InitializeAsync()
    {
        try
        {
            await context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while initializing the database");
        }
    }

    public async Task PrePopulateAsync()
    {
        try
        {
            await TryPrePopulateAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while pre-populating the database");
        }
    }

    private async Task TryPrePopulateAsync()
    {
        if (await userManager.Users.AnyAsync())
        {
            return;
        }

        Role systemAdminRole = new("SystemAdmin");
        bool addRoleSuccess = await TryAddRole(systemAdminRole);
        if (!addRoleSuccess)
        {
            return;
        }

        User systemAdminUser = new("sysadmin", "System Admin");
        bool addUserSuccess = await TryAddUser(systemAdminUser);
        if (!addUserSuccess)
        {
            return;
        }

        await TryAddUserToRole(systemAdminUser, systemAdminRole);
    }

    private async Task<bool> TryAddRole(Role role)
    {
        if (await roleManager.Roles.AnyAsync())
        {
            return true;
        }

        IdentityResult result = await roleManager.CreateAsync(role);
        if (result.Succeeded)
        {
            return true;
        }

        var errorIdx = 0;
        foreach (IdentityError error in result.Errors)
        {
            logger.LogError(
                "Pre-populating role failed #{ErrorIdx} ({ErrorCode}): {ErrorDescription}",
                errorIdx,
                error.Code,
                error.Description);
            errorIdx++;
        }

        return false;
    }

    private async Task<bool> TryAddUser(User systemAdminUser)
    {
        IdentityResult result = await userManager.CreateAsync(systemAdminUser, "sysadmin");
        if (result.Succeeded)
        {
            return true;
        }

        int errorIdx = 0;
        foreach (IdentityError error in result.Errors)
        {
            logger.LogError(
                "Pre-populating user failed #{ErrorIdx} ({ErrorCode}): {ErrorDescription}",
                errorIdx,
                error.Code,
                error.Description);
            errorIdx++;
        }

        return false;
    }

    private async Task TryAddUserToRole(User systemAdminUser, Role systemAdminRole)
    {
        IdentityResult result = await userManager.AddToRoleAsync(systemAdminUser, systemAdminRole.Name!);
        if (result.Succeeded)
        {
            return;
        }

        int errorIdx = 0;
        foreach (IdentityError error in result.Errors)
        {
            logger.LogError(
                "Adding user to role failed #{ErrorIdx} ({ErrorCode}): {ErrorDescription}",
                errorIdx,
                error.Code,
                error.Description);
            errorIdx++;
        }
    }
}