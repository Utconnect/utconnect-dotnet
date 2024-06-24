using IdentityProvider.Presentation.Middlewares;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityProvider.Presentation;

public static class ConfigureServices
{
    public static void AddPresentationServices(this IServiceCollection services)
    {
        services.AddSingleton<IAuthorizationMiddlewareResultHandler, UtconnectAuthorizationMiddlewareResultHandler>();
    }

    public static void ConfigurePresentation(this WebApplication app)
    {
        app.MapControllers();
    }
}