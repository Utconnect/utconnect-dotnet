using Jarvis.Common.Enums;
using Utconnect.Common.Models;

namespace Jarvis.Common.Services.Abstract;

public interface IFileProcessorService
{
    Task<Result<string>> StartWorkflow(WorkflowName workflow,
        Dictionary<string, object>? input,
        CancellationToken cancellationToken);
}