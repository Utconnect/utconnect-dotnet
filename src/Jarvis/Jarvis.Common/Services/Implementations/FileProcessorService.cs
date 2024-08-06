using Jarvis.Common.Enums;
using Jarvis.Common.Models;
using Jarvis.Common.Services.Abstract;
using Microsoft.Extensions.Options;
using RestSharp;
using Utconnect.Common.Models;

namespace Jarvis.Common.Services.Implementations;

internal class FileProcessorService(IOptions<FileProcessorConfig> config) : IFileProcessorService
{
    public async Task<Result<string>> StartWorkflow(WorkflowName workflow,
        Dictionary<string, object>? input,
        CancellationToken cancellationToken)
    {
        RestClientOptions options = new(config.Value.Url);
        RestClient client = new(options);
        RestRequest request = new("/workflow/start");

        RestResponse<Result<string>> response =
            await client.ExecuteGetAsync<Result<string>>(request, cancellationToken);

        return response.Data!;
    }
}