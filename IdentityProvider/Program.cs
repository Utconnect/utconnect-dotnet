using IdentityProvider;
using IdentityProvider.Application;
using IdentityProvider.Infrastructure;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

builder.Services.AddInfrastructureServices(configuration);
builder.Services.AddProviderServices(configuration);
builder.Services.AddIdentityProviderApplicationServices();

WebApplication app = builder.Build();

await app.Configure();

await app.RunAsync();