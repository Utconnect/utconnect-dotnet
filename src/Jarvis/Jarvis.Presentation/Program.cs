using Elsa.Extensions;
using Jarvis.Application;
using Jarvis.Infrastructure;
using Shared.Application.Configuration;
using Shared.Application.Configuration.Models;
using Shared.Infrastructure.Email.Configs;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

await builder.Services.AddJarvisInfrastructureServices(configuration);
builder.Services.AddJarvisApplicationServices();

builder.Services.AddConfiguration<TeacherConfig>(configuration);
builder.Services.AddConfiguration<EmailConfig>(configuration);
builder.Services.AddConfiguration<IdentityConfig>(configuration);

builder.Services.AddCors(cors => cors
    .AddDefaultPolicy(policy => policy
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod()
        .WithExposedHeaders("*")));

builder.Services.AddControllers();
builder.Services.AddHealthChecks();

WebApplication app = builder.Build();

app.UseCors();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseWorkflowsApi();
app.UseWorkflows();
app.MapControllers();
app.UseWorkflowsSignalRHubs();

app.Run();