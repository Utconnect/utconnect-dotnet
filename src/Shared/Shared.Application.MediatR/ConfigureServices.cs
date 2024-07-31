using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Application.MediatR;

public static class ConfigureServices
{
    public static void AddUtconnectMediatR(this IServiceCollection services, Assembly assembly)
    {
        services.AddMediatR(config => { config.RegisterServicesFromAssembly(assembly); });
        services.AddValidatorsFromAssembly(assembly);
    }
}