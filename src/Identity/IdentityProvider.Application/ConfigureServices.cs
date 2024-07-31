using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Shared.Application.MediatR;
using Shared.Helper;

namespace IdentityProvider.Application;

public static class ConfigureServices
{
    public static void AddIdentityApplicationServices(this IServiceCollection services)
    {
        ValidatorOptions.Global.LanguageManager.Enabled = false;
        services.AddUtconnectMediatR(Assembly.GetExecutingAssembly());
        services.AddHelpers();
    }
}