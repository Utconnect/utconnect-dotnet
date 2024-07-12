using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Oidc.Infrastructure.Persistence;
using OpenIddict.Abstractions;

namespace Oidc.Infrastructure;

public static class ConfigureServices
{
    public static void AddOidcInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<OidcDbContext>(options =>
        {
            string? connectionString = configuration.GetConnectionString("OidcDbContextConnection");
            options.UseNpgsql(connectionString);

            options.UseOpenIddict();
        });

        services.AddScoped<OidcDbContextInitializer>();

        services.AddOpenIddict()
            .AddCore(options =>
            {
                options.UseEntityFrameworkCore()
                    .UseDbContext<OidcDbContext>();
            })
            .AddServer(options =>
            {
                options.AllowClientCredentialsFlow();
                options.AllowRefreshTokenFlow();

                options.SetTokenEndpointUris("token");
                options.SetRevocationEndpointUris("token/revoke");
                options.SetIntrospectionEndpointUris("token/introspect");
                options.SetUserinfoEndpointUris("user-info");

                options.RegisterScopes(OpenIddictConstants.Scopes.Email, OpenIddictConstants.Claims.Username);
                options.SetIssuer(new Uri(configuration["Authority"] ?? string.Empty));

                options.AddDevelopmentEncryptionCertificate()
                    .AddDevelopmentSigningCertificate();
                options.DisableAccessTokenEncryption();

                options.UseAspNetCore()
                    .DisableTransportSecurityRequirement()
                    .EnableTokenEndpointPassthrough()
                    .EnableUserinfoEndpointPassthrough();
            });

        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromDays(1);
                options.LoginPath = "/login";
                options.SlidingExpiration = true;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"] ?? string.Empty))
                };
            });

        services.AddHostedService<OidcSeeder>();
    }
}