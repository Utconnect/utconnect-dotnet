using System.Text;
using Esm.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using Shared.Authentication.Services;
using Utconnect.Common;
using Utconnect.Common.Identity;
using Utconnect.Common.Infrastructure.Db.Interceptors;

namespace Esm.Infrastructure;

public static class ConfigureServices
{
    public static async Task AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCommon();
        services.AddCommonIdentity();
        services.AddScoped<AuditableEntitySaveChangesInterceptor>();

        string dbPassword = await CofferService.GetKey(configuration["Coffer"], "esm", "DB_PASSWORD");
        services.AddDbContext<EsmDbContext>(options =>
        {
            NpgsqlConnectionStringBuilder connection = new()
            {
                Host = configuration["ConnectionStringsData:EsmDbContextConnection:Host"],
                Port = int.Parse(configuration["ConnectionStringsData:EsmDbContextConnection:Port"]!),
                Username = configuration["ConnectionStringsData:EsmDbContextConnection:Username"],
                Database = configuration["ConnectionStringsData:EsmDbContextConnection:Database"],
                Password = dbPassword
            };
            options.UseNpgsql(connection.ConnectionString);
        });

        services.AddScoped<IEsmDbContext>(provider => provider.GetRequiredService<EsmDbContext>());

        services.AddScoped<EsmDbContextInitializer>();

        string jwtKey = await CofferService.GetKey(configuration["Coffer"], "esm", "JWT_KEY");
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
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
                };
            });
    }
}