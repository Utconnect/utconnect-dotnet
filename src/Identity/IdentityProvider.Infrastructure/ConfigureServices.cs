using IdentityProvider.Domain.Models;
using IdentityProvider.Infrastructure.Persistence;
using IdentityProvider.Infrastructure.Services.Abstract;
using IdentityProvider.Infrastructure.Services.Implementations;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;
using Shared.Application.Localization;
using Utconnect.Coffer.Services.Abstract;
using Utconnect.Common.Identity;
using Utconnect.Common.Models;

namespace IdentityProvider.Infrastructure;

public static class ConfigureServices
{
    public static void AddIdentityInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCommonIdentity();

        services.AddDbContext<IdentityProviderContext>((serviceProvider, options) =>
        {
            ICofferService cofferService = serviceProvider.GetService<ICofferService>()!;
            Result<string> dbPassword = cofferService.GetKey("identity", "DB_PASSWORD").GetAwaiter().GetResult();
            NpgsqlConnectionStringBuilder connection = new()
            {
                Host = configuration["ConnectionStringsData:IdentityProviderContextConnection:Host"],
                Port = int.Parse(configuration["ConnectionStringsData:IdentityProviderContextConnection:Port"]!),
                Username = configuration["ConnectionStringsData:IdentityProviderContextConnection:Username"],
                Database = configuration["ConnectionStringsData:IdentityProviderContextConnection:Database"],
                Password = dbPassword.Data
            };
            options.UseNpgsql(connection.ConnectionString);
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
            .AddCookie(options => { options.LoginPath = "/Login"; });

        services.AddTransient<IOidcService, OidcService>();
    }
}