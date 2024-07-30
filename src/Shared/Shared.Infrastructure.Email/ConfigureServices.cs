using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Infrastructure.Email.Configs;
using Shared.Infrastructure.Email.Services.Abstract;
using Shared.Infrastructure.Email.Services.Implementations;

namespace Shared.Infrastructure.Email;

public static class ConfigureServices
{
    public static void AddEmailService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IEmailService, EmailService>();
        services.Configure<EmailConfig>(configuration.GetSection(nameof(EmailConfig)));
    }
}