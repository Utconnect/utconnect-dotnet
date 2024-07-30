using Microsoft.AspNetCore.Builder;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Shared.Swashbuckle;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseUtconnectSwagger(this IApplicationBuilder app)
    {
        SwashbuckleInfo info = SwashbuckleHelper.GetInfo();

        return app.UseSwagger()
            .UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", $"{info.Title} {info.Version}");
                options.DefaultModelExpandDepth(2);
                options.DocExpansion(DocExpansion.None);
                options.EnableTryItOutByDefault();
                options.ConfigObject.AdditionalItems.Add("syntaxHighlight", false);
            });
    }
}