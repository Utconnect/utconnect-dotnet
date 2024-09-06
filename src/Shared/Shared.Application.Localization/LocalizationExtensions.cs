using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Application.Localization;

public static class LocalizationExtensions
{
    public static void AddUtconnectLocalization(this IServiceCollection services)
    {
        services.AddLocalization(options => options.ResourcesPath = "Resources");
        services.Configure<RequestLocalizationOptions>(options =>
        {
            CultureInfo[] supportedCultures =
            [
                new("en-US"), 
                new("vi-VN")
            ];

            options.DefaultRequestCulture = new RequestCulture("vi-VN");
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;
            options.RequestCultureProviders = new List<IRequestCultureProvider>
            {
                new QueryStringRequestCultureProvider(),
                new CookieRequestCultureProvider()
            };
        });
    }
}