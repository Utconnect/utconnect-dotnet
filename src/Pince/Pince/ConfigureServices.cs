using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pince.Models;
using Pince.Services.Abstract;
using Pince.Services.Implementations;
using Utconnect.Common.Configurations;

namespace Pince;

public static class ConfigureServices
{
    public static void AddFileManager(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddConfiguration<FileManagerConfig>(configuration);
        services.AddScoped<IFileManagerService, FileManagerService>();
    }
}