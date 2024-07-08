using Esm.Infrastructure.Persistence;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // Initialise and seed database
    using IServiceScope scope = app.Services.CreateScope();
    EsmDbContextInitializer initializer = scope.ServiceProvider.GetRequiredService<EsmDbContextInitializer>();
    await initializer.InitializeAsync();
}

app.UseAuthentication();

app.Run();
