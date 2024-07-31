using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Shared.Application.MediatR;

namespace Jarvis.Application;

public static class ConfigureServices
{
    public static void AddJarvisApplicationServices(this IServiceCollection services)
    {
        services.AddUtconnectMediatR(Assembly.GetExecutingAssembly());
    }
}