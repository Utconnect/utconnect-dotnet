using Microsoft.Extensions.DependencyInjection;
using Shared.Application.MediatR;

namespace Home.Application;

public static class ConfigureServices
{
    public static void ConfigureApplicationHomeServices(this IServiceCollection services)
    {
        services.AddUtconnectMediatR();
    }
}