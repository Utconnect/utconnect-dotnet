using Friday.Application;
using Friday.Infrastructure;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

await builder.Services.AddFridayInfrastructureServices(configuration);
builder.Services.AddFridayApplicationServices(configuration);

builder.Services.AddControllers();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}


app.UseRouting();
app.UseCors();

app.MapControllers();
app.MapDefaultControllerRoute();

app.Run();