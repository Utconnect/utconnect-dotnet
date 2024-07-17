using Microsoft.Extensions.Configuration;

namespace Shared.Swashbuckle;

public static class SwashbuckleHelper
{
    public static SwashbuckleInfo GetInfo()
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        string title = configuration.GetValue<string>("SwaggerSettings:Title") ?? "My API";
        string version = configuration.GetValue<string>("SwaggerSettings:Version") ?? "v1";

        return new SwashbuckleInfo(title, version);
    }
}