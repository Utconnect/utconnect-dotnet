using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OpenIddict.Abstractions;

namespace Oidc.Infrastructure.Persistence;

public class OidcDbContextInitializer(
    OidcDbContext context,
    IOpenIddictScopeManager scopeManager,
    IOpenIddictApplicationManager applicationManager,
    ILogger<OidcDbContextInitializer> logger)
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
            await SeedScopes();
            await SeedInternalApps();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database");
        }
    }

    private async ValueTask SeedScopes()
    {
        OpenIddictScopeDescriptor scopeDescriptor = new()
        {
            Name = "test_scope",
            Resources = { "test_resource" }
        };

        object? scopeInstance = await scopeManager.FindByNameAsync(scopeDescriptor.Name);

        if (scopeInstance == null)
        {
            logger.LogInformation("Creating seed scope");
            await scopeManager.CreateAsync(scopeDescriptor);
        }
        else
        {
            logger.LogInformation("Updating seed scope");
            await scopeManager.UpdateAsync(scopeInstance, scopeDescriptor);
        }
    }

    private async ValueTask SeedInternalApps()
    {
        OpenIddictApplicationDescriptor appDescriptor = new()
        {
            ClientId = "test_client",
            ClientSecret = "test_secret",
            ClientType = OpenIddictConstants.ClientTypes.Confidential,
            Permissions =
            {
                OpenIddictConstants.Permissions.Endpoints.Token,
                OpenIddictConstants.Permissions.Endpoints.Introspection,
                OpenIddictConstants.Permissions.Endpoints.Revocation,
                OpenIddictConstants.Permissions.GrantTypes.ClientCredentials,
                OpenIddictConstants.Permissions.GrantTypes.RefreshToken,
                OpenIddictConstants.Permissions.Scopes.Email,
                OpenIddictConstants.Permissions.Scopes.Profile,
                OpenIddictConstants.Permissions.Prefixes.Scope + OpenIddictConstants.Claims.Username,
                OpenIddictConstants.Permissions.Prefixes.Scope + "test_scope"
            }
        };

        object? client = await applicationManager.FindByClientIdAsync(appDescriptor.ClientId);

        if (client == null)
        {
            logger.LogInformation("Creating seed app");
            await applicationManager.CreateAsync(appDescriptor);
        }
        else
        {
            logger.LogInformation("Updating seed app");
            await applicationManager.UpdateAsync(client, appDescriptor);
        }
    }
}