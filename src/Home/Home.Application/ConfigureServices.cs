using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Utconnect.Common.MediatR;

namespace Home.Application;

public static class ConfigureServices
{
    public static void ConfigureApplicationHomeServices(this IServiceCollection services)
    {
        services.AddCommonMediatR(Assembly.GetExecutingAssembly());
    }
}