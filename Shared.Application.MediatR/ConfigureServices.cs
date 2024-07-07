using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Application.MediatR;

public static class ConfigureServices
{
    public static void AddUtconnectMediatR(this IServiceCollection services)
    {
        services.AddMediatR(config => { config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()); });
    }
}