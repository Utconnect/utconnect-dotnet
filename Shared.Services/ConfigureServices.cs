using Microsoft.Extensions.DependencyInjection;
using Shared.Services.Abstractions;
using Shared.Services.Implementations;

namespace Shared.Services;

public static class ConfigureServices
{
    public static void AddDateTime(this IServiceCollection services)
    {
        services.AddTransient<IDateTime, DateTimeService>();
    }
}