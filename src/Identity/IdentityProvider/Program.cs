using IdentityProvider;
using IdentityProvider.Application;
using IdentityProvider.Infrastructure;
using Microsoft.AspNetCore.DataProtection;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

builder.Services.AddIdentityInfrastructureServices(configuration);
builder.Services.AddIdentityServices(configuration);
builder.Services.AddIdentityApplicationServices();

builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo("/home/app/.aspnet/DataProtection-Keys"));

WebApplication app = builder.Build();

await app.Configure();

await app.RunAsync();