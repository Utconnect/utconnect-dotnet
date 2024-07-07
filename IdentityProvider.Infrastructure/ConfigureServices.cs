using IdentityProvider.Domain.Models;
using IdentityProvider.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shared.Application.Localization;
using Shared.Infrastructure.Db.Interceptors;
using Shared.Services;
using Shared.UtconnectIdentity.Services;

namespace IdentityProvider.Infrastructure;

public static class ConfigureServices
{
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<AuditableEntitySaveChangesInterceptor>();

        services.AddDbContext<IdentityProviderContext>(options =>
        {
            string? connectionString = configuration.GetConnectionString("IdentityProviderContextConnection");
            options.UseNpgsql(connectionString);
        });

        services.AddScoped<IIdentityProviderContext>(provider =>
            provider.GetRequiredService<IdentityProviderContext>());

        services.AddScoped<IdentityProviderContextInitializer>();

        services.AddDefaultIdentity<User>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.User.RequireUniqueEmail = false;
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            })
            .AddRoles<Role>()
            .AddEntityFrameworkStores<IdentityProviderContext>();

        services.TryAddScoped<SignInManager<User>>();

        services.AddUtconnectLocalization();

        services.AddRazorPages();

        services.Configure<IdentityOptions>(options =>
        {
            // Password settings.
            options.Password.RequiredLength = 6;

            // Lockout settings.
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;

            // User settings.
            options.User.RequireUniqueEmail = false;
        });

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.LoginPath = "/Login";
            });

        services.AddTransient<IIdentityService, IdentityService>();
    }
}