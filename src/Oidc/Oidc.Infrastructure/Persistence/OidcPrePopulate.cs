using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenIddict.Abstractions;

namespace Oidc.Infrastructure.Persistence;

public class OidcPrePopulate(ILogger<OidcPrePopulate> logger, IServiceProvider serviceProvider) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using IServiceScope scope = serviceProvider.CreateScope();

        await PrePopulateScopes(scope, cancellationToken);
        await PrePopulateInternalApps(scope, cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private async ValueTask PrePopulateScopes(IServiceScope scope, CancellationToken cancellationToken)
    {
        IOpenIddictScopeManager scopeManager = scope.ServiceProvider.GetRequiredService<IOpenIddictScopeManager>();

        OpenIddictScopeDescriptor scopeDescriptor = new()
        {
            Name = "test_scope",
            Resources = { "test_resource" }
        };

        object? scopeInstance = await scopeManager.FindByNameAsync(scopeDescriptor.Name, cancellationToken);

        if (scopeInstance == null)
        {
            logger.LogInformation("Creating pre-populate scope");
            await scopeManager.CreateAsync(scopeDescriptor, cancellationToken);
        }
        else
        {
            logger.LogInformation("Updating pre-populate scope");
            await scopeManager.UpdateAsync(scopeInstance, scopeDescriptor, cancellationToken);
        }
    }

    private async ValueTask PrePopulateInternalApps(IServiceScope scopeService, CancellationToken cancellationToken)
    {
        IOpenIddictApplicationManager appManager =
            scopeService.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();

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

        object? client = await appManager.FindByClientIdAsync(appDescriptor.ClientId, cancellationToken);

        if (client == null)
        {
            logger.LogInformation("Creating pre-populate app");
            await appManager.CreateAsync(appDescriptor, cancellationToken);
        }
        else
        {
            logger.LogInformation("Updating pre-populate app");
            await appManager.UpdateAsync(client, appDescriptor, cancellationToken);
        }
    }
}