using System.Text;
using Esm.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Shared.Infrastructure.Db.Interceptors;
using Shared.Services;
using Shared.UtconnectIdentity.Services;

namespace Esm.Infrastructure;

public static class ConfigureServices
{
    public static void AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<AuditableEntitySaveChangesInterceptor>();

        services.AddDbContext<EsmDbContext>(options =>
        {
            string? connectionString = configuration.GetConnectionString("EsmDbContextConnection");
            options.UseNpgsql(connectionString);
        });

        services.AddScoped<IEsmDbContext>(provider => provider.GetRequiredService<EsmDbContext>());

        services.AddScoped<EsmDbContextInitializer>();

        services.AddDateTime();
        services.AddTransient<IIdentityService, IdentityService>();
        
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
           .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidAudience = configuration["Jwt:Audience"],
                    ValidIssuer = configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!))
                };
            });
    }
}