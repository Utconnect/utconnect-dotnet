using Sso.Presentation;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSsoPresentationServices();

WebApplication app = builder.Build();

app.Configure();
app.Run();