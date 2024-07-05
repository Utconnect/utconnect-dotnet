using Oidc.Infrastructure;
using Oidc.Presentation;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddOidcInfrastructureServices(builder.Configuration);
builder.Services.AddOidcPresentationServices();

WebApplication app = builder.Build();

app.Configure();
app.Run();