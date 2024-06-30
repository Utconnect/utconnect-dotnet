using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
    .AddJsonFile("ocelot.json")
    .AddEnvironmentVariables();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    {
        // Cookie settings
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromHours(6);

        options.LoginPath = "/auth/login";
        options.AccessDeniedPath = "/accessDenied";
        options.SlidingExpiration = true;
    })
    .AddCookie(IdentityConstants.ApplicationScheme, options =>
    {
        // Cookie settings
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromHours(6);

        options.LoginPath = "/auth/login";
        options.AccessDeniedPath = "/accessDenied";
        options.SlidingExpiration = true;
    });


builder.Services.AddOcelot();

WebApplication app = builder.Build();

OcelotPipelineConfiguration configuration = new()
{
    PreAuthenticationMiddleware = async (_, next) =>
    {
        Console.WriteLine("PreAuth");
        await next.Invoke();
    },
    AuthenticationMiddleware = async (_, next) =>
    {
        Console.WriteLine("Auth");
        await next.Invoke();
    } 
};

app.UseOcelot(configuration).Wait();

app.Run();