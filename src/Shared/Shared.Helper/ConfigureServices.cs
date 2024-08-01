using Diacritics;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Helper;

public static class ConfigureServices
{
    public static void AddHelpers(this IServiceCollection services)
    {
        services.AddTransient<IDiacriticsMapper, UtconnectDiacriticsMapper>();
        services.AddTransient<StringHelper>();
    }
}