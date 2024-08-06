using Microsoft.AspNetCore.Authentication.Cookies;
using Shared.Application.Configuration.Models;
using Shared.Application.Localization;
using Utconnect.Common;
using Utconnect.Common.Configurations;

namespace Home.Presentation;

public static class ConfigureServices
{
    public static void ConfigureHomePresentationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCommon();
        services.AddRazorPages();
        services.AddConfiguration<TssConfig>(configuration);
        services.AddConfiguration<EsmConfig>(configuration);
        services.AddConfiguration<OidcConfig>(configuration);
        services.AddConfiguration<IdentityConfig>(configuration);
        services.AddUtconnectLocalization();

        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.Events.OnRedirectToLogin = context =>
                {
                    context.HttpContext.Response.Redirect(configuration.GetSection("HomeConfig")["Url"]!);
                    return Task.CompletedTask;
                };
            });
    }
}