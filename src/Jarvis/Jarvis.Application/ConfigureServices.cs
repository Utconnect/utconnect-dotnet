using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Utconnect.Common.MediatR;

namespace Jarvis.Application;

public static class ConfigureServices
{
    public static void AddJarvisApplicationServices(this IServiceCollection services)
    {
        services.AddCommonMediatR(Assembly.GetExecutingAssembly());
    }
}