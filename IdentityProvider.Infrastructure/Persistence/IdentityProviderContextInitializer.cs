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

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database");
        }
    }

    private async Task TrySeedAsync()
    {
        if (await userManager.Users.AnyAsync())
        {
            return;
        }

        var systemAdminRole = new Role("SystemAdmin");
        if (!await roleManager.Roles.AnyAsync())
        {
            var createRoleResult = await roleManager.CreateAsync(systemAdminRole);
            if (!createRoleResult.Succeeded)
            {
                var errorIdx = 0;
                foreach (var error in createRoleResult.Errors)
                {
                    logger.LogError(
                        "Seeding role failed #{ErrorIdx} ({ErrorCode}): {ErrorDescription}",
                        errorIdx,
                        error.Code,
                        error.Description);
                    errorIdx++;
                }

                return;
            }
        }

        var systemAdminUser = new User("sysadmin", "System Admin");
        var createUserResult = await userManager.CreateAsync(systemAdminUser, "sysadmin");
        if (!createUserResult.Succeeded)
        {
            var errorIdx = 0;
            foreach (var error in createUserResult.Errors)
            {
                logger.LogError(
                    "Seeding user failed #{ErrorIdx} ({ErrorCode}): {ErrorDescription}",
                    errorIdx,
                    error.Code,
                    error.Description);
                errorIdx++;
            }
        }

        await userManager.AddToRoleAsync(systemAdminUser, systemAdminRole.Name!);
    }
}