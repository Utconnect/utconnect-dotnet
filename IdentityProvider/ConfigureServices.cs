using IdentityProvider.Infrastructure.Persistence;
using Shared.Presentation.Filters;

namespace IdentityProvider;

public static class ConfigureServices
{
    public static void AddPresentationServices(this IServiceCollection services)
    {
        services.AddControllersWithViews();
        // services.AddUtconnectSwashbuckle();
        services.AddHttpContextAccessor();

        services.AddMvc(options => { options.Filters.Add<HttpResponseExceptionFilter>(); })
            .ConfigureApiBehaviorOptions(options => { options.SuppressModelStateInvalidFilter = true; });
        
        // services.AddControllers().AddNewtonsoftJson(options =>
        //     options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
    }

    public static async Task Configure(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            // app.UseUtconnectSwagger();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        using (var scope = app.Services.CreateScope())
        {
            var initializer = scope.ServiceProvider.GetRequiredService<IdentityProviderContextInitializer>();
            await initializer.InitializeAsync();
            await initializer.SeedAsync();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapRazorPages();
    }
}