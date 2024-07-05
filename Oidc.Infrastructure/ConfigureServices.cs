using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Oidc.Infrastructure.Persistence;

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

        services.AddOpenIddict()
            .AddCore(options =>
            {
                options.UseEntityFrameworkCore()
                    .UseDbContext<OidcDbContext>();
            })
            .AddServer(options =>
            {
                //enable client_credentials grant_tupe support on server level
                options.AllowClientCredentialsFlow();
                //specify token endpoint uri
                options.SetTokenEndpointUris("token");
                options.SetIntrospectionEndpointUris("token/introspect");
                options.SetRevocationEndpointUris("token/revoke");
                options.AllowRefreshTokenFlow();
                //secret registration
                options.AddDevelopmentEncryptionCertificate()
                    .AddDevelopmentSigningCertificate();
                options.DisableAccessTokenEncryption();
                //the asp request handlers configuration itself
                options.UseAspNetCore().EnableTokenEndpointPassthrough();
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