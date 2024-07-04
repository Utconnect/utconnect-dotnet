using IdentityProvider.Infrastructure.Persistence;
using Microsoft.Extensions.Options;
using Shared.Application.Configuration;
using Shared.Swashbuckle;

namespace IdentityProvider;

public static class ConfigureServices
{
    public static void AddProviderServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllersWithViews();
        services.AddUtconnectSwashbuckle();
        services.AddHttpContextAccessor();

        services.Configure<HomeConfig>(configuration.GetSection("HomeConfig"));
        services.Configure<OidcConfig>(configuration.GetSection("OidcConfig"));
    }

    public static async Task Configure(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseUtconnectSwagger();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        using (IServiceScope scope = app.Services.CreateScope())
        {
            IdentityProviderContextInitializer initializer =
                scope.ServiceProvider.GetRequiredService<IdentityProviderContextInitializer>();
            await initializer.InitializeAsync();
            await initializer.SeedAsync();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapRazorPages();
    }
}