using IdentityProvider.Application.Services.Abstract;
using IdentityProvider.Application.Services.Implementations;
using Microsoft.Extensions.DependencyInjection;
using Shared.Services;

namespace IdentityProvider.Application;

public static class ConfigureServices
{
    public static void AddIdentityProviderApplicationServices(this IServiceCollection services)
    {
        services.AddTransient<IOidcService, OidcService>();
        services.AddDateTime();
    }
}