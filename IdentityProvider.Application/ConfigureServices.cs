using IdentityProvider.Application.Services.Abstract;
using IdentityProvider.Application.Services.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityProvider.Application;

public static class ConfigureServices
{
    public static void AddIdentityProviderApplicationServices(this IServiceCollection services)
    {
        services.AddTransient<IOidcService, OidcService>();
    }
}