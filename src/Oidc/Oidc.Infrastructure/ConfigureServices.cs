using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using Oidc.Infrastructure.Persistence;
using OpenIddict.Abstractions;
using Shared.Authentication.Services;

namespace Oidc.Infrastructure;

public static class ConfigureServices
{
    public static async Task AddOidcInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        string dbPassword = await CofferService.GetKey(configuration["Coffer"], "oidc", "DB_PASSWORD");
        services.AddDbContext<OidcDbContext>(options =>
        {
            NpgsqlConnectionStringBuilder connection = new()
            {
                Host = configuration["ConnectionStringsData:OidcDbContextConnection:Host"],
                Port = int.Parse(configuration["ConnectionStringsData:OidcDbContextConnection:Port"]!),
                Username = configuration["ConnectionStringsData:OidcDbContextConnection:Username"],
                Database = configuration["ConnectionStringsData:OidcDbContextConnection:Database"],
                Password = dbPassword
            };
            options.UseNpgsql(connection.ConnectionString);

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

                options.SetTokenEndpointUris("connect/token");
                options.SetRevocationEndpointUris("connect/token/revoke");
                options.SetIntrospectionEndpointUris("connect/token/introspect");
                options.SetUserinfoEndpointUris("connect/user-info");

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

        string jwtKey = await CofferService.GetKey(configuration["Coffer"], "oidc", "JWT_KEY");

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
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
                };
            });

        services.AddHostedService<OidcPrePopulate>();
    }
}