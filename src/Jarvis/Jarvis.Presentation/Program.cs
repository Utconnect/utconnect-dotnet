using Elsa.Extensions;
using Jarvis.Application;
using Jarvis.Infrastructure;
using Shared.Application.Configuration.Models;
using Shared.Infrastructure.Email.Configs;
using Utconnect.Common.Configurations;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

await builder.Services.AddJarvisInfrastructureServices(configuration);
builder.Services.AddJarvisApplicationServices();

builder.Services.AddConfiguration<TeacherConfig>(configuration);
builder.Services.AddConfiguration<EmailConfig>(configuration);
builder.Services.AddConfiguration<IdentityConfig>(configuration);

const string localhostOrigins = "localhostOrigins";

builder.Services.AddCors(cors =>
{
    cors.AddPolicy(name: localhostOrigins,
        policy =>
        {
            policy
                .WithOrigins("http://localhost:5254")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .WithExposedHeaders("*");
        });
});

builder.Services.AddControllers();
builder.Services.AddHealthChecks();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseCors(localhostOrigins);
}

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseWorkflowsApi();
app.UseWorkflows();
app.MapControllers();
app.UseWorkflowsSignalRHubs();

await app.RunAsync();