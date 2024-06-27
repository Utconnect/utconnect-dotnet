using Ocelot.DependencyInjection;
using Ocelot.Middleware;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddOcelot();

WebApplication app = builder.Build();

app.UseOcelot().Wait();

app.Run();