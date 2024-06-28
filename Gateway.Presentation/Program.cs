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

builder.Services.AddAuthentication()
    .AddJwtBearer("TestKey", opt =>
    {
        opt.Authority = "https://localhost:6001";
        opt.Audience = "SampleApiService";
        opt.RequireHttpsMetadata = false;
    });


builder.Services.AddOcelot();

WebApplication app = builder.Build();

OcelotPipelineConfiguration configuration = new OcelotPipelineConfiguration
{
    PreAuthenticationMiddleware = async (context, next) =>
    {
        await next.Invoke();
    },
    AuthenticationMiddleware = async (context, next) =>
    {
        await next.Invoke();
    } 
};

app.UseOcelot(configuration).Wait();

app.Run();