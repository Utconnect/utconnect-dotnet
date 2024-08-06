using Friday.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Shared.Authentication.Services;

namespace Friday.Infrastructure;

public static class ConfigureServices
{
    public static async Task AddFridayInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        string dbPassword = await CofferService.GetKey(configuration["Coffer"], "data_processor", "DB_PASSWORD");
        services.AddDbContext<FridayDbContext>(options =>
        {
            NpgsqlConnectionStringBuilder connection = new()
            {
                Host = configuration["ConnectionStringsData:FridayDbContextConnection:Host"],
                Port = int.Parse(configuration["ConnectionStringsData:FridayDbContextConnection:Port"]!),
                Username = configuration["ConnectionStringsData:FridayDbContextConnection:Username"],
                Database = configuration["ConnectionStringsData:FridayDbContextConnection:Database"],
                Password = dbPassword
            };
            options.UseNpgsql(connection.ConnectionString);
        });
    }
}