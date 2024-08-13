using System.Text;
using Esm.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using Utconnect.Coffer;
using Utconnect.Coffer.Services.Abstract;
using Utconnect.Common;
using Utconnect.Common.Identity;
using Utconnect.Common.Infrastructure.Db.Interceptors;
using Utconnect.Common.Models;

namespace Esm.Infrastructure;

public static class ConfigureServices
{
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCommon();
        services.AddCommonIdentity();
        services.AddCoffer(configuration);
        services.AddScoped<AuditableEntitySaveChangesInterceptor>();

        services.AddDbContext<EsmDbContext>((serviceProvider, options) =>
        {
            ICofferService cofferService = serviceProvider.GetService<ICofferService>()!;
            Result<string> dbPassword = cofferService.GetKey("esm", "DB_PASSWORD").GetAwaiter().GetResult();

            NpgsqlConnectionStringBuilder connection = new()
            {
                Host = configuration["ConnectionStringsData:EsmDbContextConnection:Host"],
                Port = int.Parse(configuration["ConnectionStringsData:EsmDbContextConnection:Port"]!),
                Username = configuration["ConnectionStringsData:EsmDbContextConnection:Username"],
                Database = configuration["ConnectionStringsData:EsmDbContextConnection:Database"],
                Password = dbPassword.Data
            };
            options.UseNpgsql(connection.ConnectionString);
        });

        services.AddScoped<IEsmDbContext>(provider => provider.GetRequiredService<EsmDbContext>());

        services.AddScoped<EsmDbContextInitializer>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                ICofferService cofferService = services.BuildServiceProvider().GetService<ICofferService>()!;
                Result<string> jwtKey = cofferService.GetKey("esm", "JWT_KEY").GetAwaiter().GetResult();

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidAudience = configuration["Jwt:Audience"],
                    ValidIssuer = configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey.Data ?? string.Empty))
                };
            });
    }
}