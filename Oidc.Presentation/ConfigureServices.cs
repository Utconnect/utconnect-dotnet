namespace Oidc.Presentation;

public static class ConfigureServices
{
    public static void AddOidcPresentationServices(this IServiceCollection services)
    {
        services.AddControllers();
    }

    public static void Configure(this WebApplication app)
    {
        app.UseDeveloperExceptionPage();

        app.UseRouting();
        app.UseCors();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
        app.MapDefaultControllerRoute();
    }
}