using IdentityProvider;
using IdentityProvider.Infrastructure;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddPresentationServices(builder.Configuration);

WebApplication app = builder.Build();

await app.Configure();

await app.RunAsync();