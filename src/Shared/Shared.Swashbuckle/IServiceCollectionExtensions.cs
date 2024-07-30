using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Shared.Swashbuckle.Filters;
using Unchase.Swashbuckle.AspNetCore.Extensions.Extensions;

namespace Shared.Swashbuckle;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddUtconnectSwashbuckle(this IServiceCollection services)
    {
        SwashbuckleInfo info = SwashbuckleHelper.GetInfo();

        return services.AddSwaggerGen(options =>
        {
            options.CustomSchemaIds(i => i.FullName);
            options.SwaggerDoc("v1", new OpenApiInfo { Title = info.Title, Version = info.Version });
            options.SupportNonNullableReferenceTypes();
            options.UseOneOfForPolymorphism();
            options.SchemaFilter<SwaggerRequiredSchemaFilter>();
            options.AddEnumsWithValuesFixFilters(opt =>
            {
                opt.ApplySchemaFilter = true;
                opt.XEnumNamesAlias = "x-enum-varnames";
                opt.XEnumDescriptionsAlias = "x-enum-descriptions";
            });
        });
    }
}