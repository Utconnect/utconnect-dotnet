using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Shared.Swashbuckle.Filters;

public class SwaggerRequiredSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (schema.Properties == null)
            return;

        foreach (KeyValuePair<string, OpenApiSchema> schemaProp in schema.Properties)
        {
            if (schemaProp.Value.Nullable)
                continue;

            schema.Required.Add(schemaProp.Key);
        }
    }
}