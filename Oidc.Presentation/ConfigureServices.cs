using Oidc.Infrastructure.Persistence;

namespace Oidc.Presentation;

public static class ConfigureServices
{
    public static void AddOidcPresentationServices(this IServiceCollection services)
    {
        services.AddControllers();
    }

    public static async Task Configure(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        using (IServiceScope scope = app.Services.CreateScope())
        {
            OidcDbContextInitializer initializer = scope.ServiceProvider.GetRequiredService<OidcDbContextInitializer>();
            await initializer.InitializeAsync();
            await initializer.SeedAsync();
        }

        app.UseRouting();
        app.UseCors();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
        app.MapDefaultControllerRoute();
    }
}