using Microsoft.Extensions.DependencyInjection;
using Shared.Infrastructure.Email.Services.Abstract;
using Shared.Infrastructure.Email.Services.Implementations;

namespace Shared.Infrastructure.Email;

public static class ConfigureServices
{
    public static void AddEmailService(this IServiceCollection services)
    {
        services.AddTransient<IEmailService, EmailService>();
    }
}