using Microsoft.AspNetCore.DataProtection;
using Oidc.Infrastructure;
using Oidc.Presentation;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddOidcInfrastructureServices(builder.Configuration);
builder.Services.AddOidcPresentationServices();

builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo("/home/app/.aspnet/DataProtection-Keys"));

WebApplication app = builder.Build();

app.Configure();
app.Run();