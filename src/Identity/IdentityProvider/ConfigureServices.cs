using IdentityProvider.Infrastructure.Persistence;
using Microsoft.Extensions.Options;
using Shared.Application.Configuration;
using Shared.Application.Configuration.Models;
using Shared.Authentication.Services;
using Shared.Infrastructure.Email;
using Shared.Services.Abstractions;
using Shared.Swashbuckle;

namespace IdentityProvider;

public static class ConfigureServices
{
    public static async Task AddProviderServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllersWithViews();
        services.AddUtconnectSwashbuckle();
        services.AddHttpContextAccessor();
        services.AddEmailService(configuration);

        services.AddConfiguration<HomeConfig>(configuration);
        services.AddConfiguration<OidcConfig>(configuration);

        string jwtKey = await CofferService.GetKey(configuration["Coffer"], "oidc", "JWT_KEY");
        IConfigurationSection jwtConfig = configuration.GetSection("OidcConfig:Jwt");
        services.AddTransient<IJwtService>(serviceProvider =>
        {
            IDateTime dateTime = serviceProvider.GetService<IDateTime>()!;
            return new JwtService(jwtConfig, dateTime, jwtKey);
        });
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

        app.UseStaticFiles();

        app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapRazorPages();
    }
}