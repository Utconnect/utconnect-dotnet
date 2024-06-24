using IdentityProvider;
using IdentityProvider.Infrastructure;
using IdentityProvider.Presentation;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddProviderServices(builder.Configuration);
builder.Services.AddPresentationServices();

WebApplication app = builder.Build();

await app.Configure();
app.ConfigurePresentation();

await app.RunAsync();