using Jarvis.Common.Enums;
using MediatR;
using Utconnect.Common.Models;

namespace Jarvis.Application.Workflows.StartWorkflow;

public record StartWorkflowCommand(WorkflowName Workflow, Dictionary<string, object>? Input) : IRequest<Result<string>>;