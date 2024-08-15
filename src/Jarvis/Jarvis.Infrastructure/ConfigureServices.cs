using Elsa.EntityFrameworkCore.Extensions;
using Elsa.EntityFrameworkCore.Modules.Management;
using Elsa.EntityFrameworkCore.Modules.Runtime;
using Elsa.Extensions;
using Jarvis.Domain.Workflows.User.AddNewTeacher;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Utconnect.Coffer;
using Utconnect.Coffer.Services.Abstract;
using Utconnect.Common.Models;

namespace Jarvis.Infrastructure;

public static class ConfigureServices
{
    public static void AddJarvisInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCoffer(configuration);

        services.AddElsa(elsa =>
        {
            ICofferService cofferService = services.BuildServiceProvider().GetService<ICofferService>()!;
            Result<string> dbPassword = cofferService.GetKey("jarvis", "DB_PASSWORD").GetAwaiter().GetResult();
            NpgsqlConnectionStringBuilder dbConnection = new()
            {
                Host = configuration["ConnectionStringsData:JarvisDbContextConnection:Host"],
                Port = int.Parse(configuration["ConnectionStringsData:JarvisDbContextConnection:Port"]!),
                Username = configuration["ConnectionStringsData:JarvisDbContextConnection:Username"],
                Database = configuration["ConnectionStringsData:JarvisDbContextConnection:Database"],
                Password = dbPassword.Data
            };

            elsa.UseWorkflowManagement(feature =>
                feature.UseEntityFrameworkCore(ef => ef.UsePostgreSql(dbConnection.ConnectionString)));

            elsa.UseWorkflowRuntime(feature =>
            {
                feature.UseEntityFrameworkCore(ef => ef.UsePostgreSql(dbConnection.ConnectionString));
                feature.AddWorkflow<AddNewTeacherWorkflow>();
            });

            elsa.UseIdentity(identity =>
            {
                identity.TokenOptions = options => options.SigningKey = "sufficiently-large-secret-signing-key";
                identity.UseAdminUserProvider();
            });

            elsa.UseDefaultAuthentication(auth => auth.UseAdminApiKey());

            elsa.UseWorkflowsApi();

            // Set up a SignalR hub for real-time updates from the server.
            elsa.UseRealTimeWorkflows();

            // Enable C# workflow expressions
            elsa.UseCSharp();

            elsa.UseHttp();
        });
    }
}