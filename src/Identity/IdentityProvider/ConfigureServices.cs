using IdentityProvider.Infrastructure.Persistence;
using Microsoft.Extensions.Options;
using Shared.Application.Configuration.Models;
using Shared.Authentication.Services;
using Shared.Infrastructure.Email;
using Shared.Swashbuckle;
using Utconnect.Coffer;
using Utconnect.Coffer.Services.Abstract;
using Utconnect.Common;
using Utconnect.Common.Configurations;
using Utconnect.Common.Exceptions.Filters;
using Utconnect.Common.Helpers;
using Utconnect.Common.Models;
using Utconnect.Common.Services.Abstractions;

namespace IdentityProvider;

public static class ConfigureServices
{
    public static void AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllersWithViews();
        services.AddUtconnectSwashbuckle();
        services.AddHttpContextAccessor();
        services.AddCoffer(configuration);

        services.AddEmailService(configuration);
        services.AddHelpers();
        services.AddCommon();

        services.AddConfiguration<HomeConfig>(configuration);
        services.AddConfiguration<OidcConfig>(configuration);

        IConfigurationSection jwtConfig = configuration.GetSection("OidcConfig:Jwt");
        services.AddTransient<IJwtService>(serviceProvider =>
        {
            ICofferService cofferService = serviceProvider.GetService<ICofferService>()!;
            Result<string> jwtKey = cofferService.GetKey("oidc", "JWT_KEY").GetAwaiter().GetResult();
            IDateTime dateTime = serviceProvider.GetService<IDateTime>()!;

            return new JwtService(jwtConfig, dateTime, jwtKey.Data);
        });

        services.AddTransient<RedirectService>();
        services.AddScoped<HttpResponseExceptionFilter>();
        services.AddHealthChecks();
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
            await initializer.PrePopulateAsync();
        }

        app.UseStaticFiles();

        app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

        app.UseRouting();
        app.MapControllers();
        app.MapHealthChecks("/api/health");

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapRazorPages();
    }
}