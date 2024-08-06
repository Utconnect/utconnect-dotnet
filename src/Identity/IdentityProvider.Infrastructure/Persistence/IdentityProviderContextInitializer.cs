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
    private const string RoleAdmin = "SystemAdmin";
    private const string RoleTeacher = "Teacher";

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
            await TryPrePopulateAdminAsync();
            await TryPrePopulateTeacherAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while pre-populating the database");
        }
    }

    #region PrePopulateAdmin

    private async Task TryPrePopulateAdminAsync()
    {
        if (await userManager.Users.AnyAsync())
        {
            return;
        }

        Role systemAdminRole = new(RoleAdmin);
        bool addRoleSuccess = await TryAddAdminRole(systemAdminRole);
        if (!addRoleSuccess)
        {
            return;
        }

        User systemAdminUser = new("sysadmin", "System Admin");
        bool addUserSuccess = await TryAddAdminUser(systemAdminUser);
        if (!addUserSuccess)
        {
            return;
        }

        await TryAddAdminUserToRole(systemAdminUser, systemAdminRole);
    }

    private async Task<bool> TryAddAdminRole(Role role)
    {
        if (await roleManager.FindByNameAsync(RoleAdmin) != null)
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

    private async Task<bool> TryAddAdminUser(User systemAdminUser)
    {
        IdentityResult result = await userManager.CreateAsync(systemAdminUser, "sysadmin");
        if (result.Succeeded)
        {
            return true;
        }

        var errorIdx = 0;
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

    private async Task TryAddAdminUserToRole(User systemAdminUser, Role systemAdminRole)
    {
        IdentityResult result = await userManager.AddToRoleAsync(systemAdminUser, systemAdminRole.Name!);
        if (result.Succeeded)
        {
            return;
        }

        var errorIdx = 0;
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

    #endregion

    #region PrePopulateTeacher

    private async Task TryPrePopulateTeacherAsync()
    {
        Role teacherRole = new(RoleTeacher);
        await TryAddTeacherRole(teacherRole);
    }

    private async Task TryAddTeacherRole(Role role)
    {
        if (await roleManager.FindByNameAsync(RoleTeacher) != null)
        {
            return;
        }

        IdentityResult result = await roleManager.CreateAsync(role);
        if (result.Succeeded)
        {
            return;
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
    }

    #endregion
}