using Friday.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Utconnect.Coffer;
using Utconnect.Coffer.Services.Abstract;
using Utconnect.Common.Models;

namespace Friday.Infrastructure;

public static class ConfigureServices
{
    public static void AddFridayInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCoffer(configuration);
        
        services.AddDbContext<FridayDbContext>((serviceProvider, options) =>
        {
            ICofferService cofferService = serviceProvider.GetService<ICofferService>()!;
            Result<string> dbPassword = cofferService.GetKey("data_processor", "DB_PASSWORD").GetAwaiter().GetResult();
            NpgsqlConnectionStringBuilder connection = new()
            {
                Host = configuration["ConnectionStringsData:FridayDbContextConnection:Host"],
                Port = int.Parse(configuration["ConnectionStringsData:FridayDbContextConnection:Port"]!),
                Username = configuration["ConnectionStringsData:FridayDbContextConnection:Username"],
                Database = configuration["ConnectionStringsData:FridayDbContextConnection:Database"],
                Password = dbPassword.Data
            };
            options.UseNpgsql(connection.ConnectionString);
        });
    }
}