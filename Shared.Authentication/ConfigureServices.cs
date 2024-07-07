using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Authentication.Services;
using Shared.Services.Abstractions;

namespace Shared.Authentication;

public static class ConfigureServices
{
    public static void AddDefaultJwtService(this IServiceCollection services, string jwtSettingSection)
    {
        services.AddTransient<IJwtService>(serviceProvider =>
        {
            IConfiguration config = serviceProvider.GetService<IConfiguration>()!;
            IDateTime dateTime = serviceProvider.GetService<IDateTime>()!;
            IConfigurationSection jwtConfig = config.GetSection(jwtSettingSection);

            return new JwtService(jwtConfig, dateTime);
        });
    }
}