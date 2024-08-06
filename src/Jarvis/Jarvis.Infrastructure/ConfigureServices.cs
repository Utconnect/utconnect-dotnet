using Elsa.EntityFrameworkCore.Extensions;
using Elsa.EntityFrameworkCore.Modules.Management;
using Elsa.EntityFrameworkCore.Modules.Runtime;
using Elsa.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Shared.Authentication.Services;
using Jarvis.Domain.Workflows.User.AddNewTeacher;

namespace Jarvis.Infrastructure;

public static class ConfigureServices
{
    public static async Task AddJarvisInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        string dbPassword = await CofferService.GetKey(configuration["Coffer"], "jarvis", "DB_PASSWORD");

        services.AddElsa(elsa =>
        {
            NpgsqlConnectionStringBuilder dbConnection = new()
            {
                Host = configuration["ConnectionStringsData:JarvisDbContextConnection:Host"],
                Port = int.Parse(configuration["ConnectionStringsData:JarvisDbContextConnection:Port"]!),
                Username = configuration["ConnectionStringsData:JarvisDbContextConnection:Username"],
                Database = configuration["ConnectionStringsData:JarvisDbContextConnection:Database"],
                Password = dbPassword
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