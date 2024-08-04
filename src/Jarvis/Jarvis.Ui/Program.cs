using Elsa.Studio.Dashboard.Extensions;
using Elsa.Studio.Shell;
using Elsa.Studio.Shell.Extensions;
using Elsa.Studio.Workflows.Extensions;
using Elsa.Studio.Contracts;
using Elsa.Studio.Core.BlazorWasm.Extensions;
using Elsa.Studio.Extensions;
using Elsa.Studio.Login.BlazorWasm.Extensions;
using Elsa.Studio.Login.HttpMessageHandlers;
using Elsa.Studio.Workflows.Designer.Extensions;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

// Build the host.
WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);
WebAssemblyHostConfiguration configuration = builder.Configuration;

// Register root components.
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.RootComponents.RegisterCustomElsaStudioElements();

// Register shell services and modules.
builder.Services.AddCore();
builder.Services.AddShell();
builder.Services.AddRemoteBackend(
    elsaClient => elsaClient.AuthenticationHandler = typeof(AuthenticatingApiHttpMessageHandler),
    options => configuration.GetSection("Backend").Bind(options));
builder.Services.AddLoginModule();
builder.Services.AddDashboardModule();
builder.Services.AddWorkflowsModule();
// builder.Services.AddResponseCompression();


// Build the application.
WebAssemblyHost app = builder.Build();

// Run each startup task.
IStartupTaskRunner startupTaskRunner = app.Services.GetRequiredService<IStartupTaskRunner>();
await startupTaskRunner.RunStartupTasksAsync();

// Run the application.
await app.RunAsync();