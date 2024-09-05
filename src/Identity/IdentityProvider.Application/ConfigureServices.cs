using System.Reflection;
using FluentValidation;
using IdentityProvider.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Utconnect.Common.MediatR;
using Utconnect.Teacher;

namespace IdentityProvider.Application;

public static class ConfigureServices
{
    public static void AddIdentityApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTeacherService(configuration);

        ValidatorOptions.Global.LanguageManager.Enabled = false;
        services.AddCommonMediatR(Assembly.GetExecutingAssembly());

        services.AddTransient<UserService>();
    }
}