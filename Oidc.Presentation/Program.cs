using Oidc.Infrastructure;
using Oidc.Presentation;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddOidcInfrastructureServices(builder.Configuration);
builder.Services.AddOidcPresentationServices();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer();

WebApplication app = builder.Build();

app.Configure();
app.Run();