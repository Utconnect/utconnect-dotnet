using Microsoft.Extensions.Configuration;

namespace Shared.Swashbuckle;

public static class SwashbuckleHelper
{
    public static SwashbuckleInfo GetInfo()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var title = configuration.GetValue<string>("SwaggerSettings:Title") ?? "My API";
        var version = configuration.GetValue<string>("SwaggerSettings:Version") ?? "v1";

        return new SwashbuckleInfo(title, version);
    }
}