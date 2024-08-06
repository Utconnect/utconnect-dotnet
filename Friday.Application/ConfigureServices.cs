using System.Reflection;
using Jarvis.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pince;
using Shared.Application.MediatR;

namespace Friday.Application;

public static class ConfigureServices
{
    public static void AddFridayApplicationServices(this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.AddUtconnectMediatR(Assembly.GetExecutingAssembly());
        services.AddFileProcessor(configuration);
        services.AddFileManager(configuration);
    }
}