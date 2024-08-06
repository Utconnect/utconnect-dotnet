using Jarvis.Common.Models;
using Jarvis.Common.Services.Abstract;
using Jarvis.Common.Services.Implementations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Application.Configuration;

namespace Jarvis.Common;

public static class ConfigureServices
{
    public static void AddFileProcessor(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddConfiguration<FileProcessorConfig>(configuration);
        services.AddScoped<IFileProcessorService, FileProcessorService>();
    }
}