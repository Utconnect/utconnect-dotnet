using Jarvis.Common.Enums;
using Shared.Presentation.Models;

namespace Jarvis.Common.Services.Abstract;

public interface IFileProcessorService
{
    Task<Result<string>> StartWorkflow(WorkflowName workflow,
        Dictionary<string, object>? input,
        CancellationToken cancellationToken);
}