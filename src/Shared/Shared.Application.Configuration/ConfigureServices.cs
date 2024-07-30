using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Application.Configuration;

public static class ConfigureServices
{
    public static void AddConfiguration<TOptions>(this IServiceCollection services, IConfiguration configuration)
        where TOptions : class
    {
        services.Configure<TOptions>(configuration.GetSection(typeof(TOptions).Name));
    }
}