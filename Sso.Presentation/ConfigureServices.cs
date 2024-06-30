using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using Shared.Application.Localization;

namespace Sso.Presentation;

public static class ConfigureServices
{
    public static void AddSsoPresentationServices(this IServiceCollection services)
    {
        services.AddUtconnectLocalization();
        services.AddControllers();
        services.AddRazorPages();

        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/Login";
            });
    }

    public static void Configure(this WebApplication app)
    {
        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Exchange}/{action=Index}/{id?}");
    }
}