using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Utconnect.Common.MediatR;

namespace IdentityProvider.Application;

public static class ConfigureServices
{
    public static void AddIdentityApplicationServices(this IServiceCollection services)
    {
        ValidatorOptions.Global.LanguageManager.Enabled = false;
        services.AddCommonMediatR(Assembly.GetExecutingAssembly());
    }
}