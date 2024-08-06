using Jarvis.Common.Enums;
using MediatR;
using Shared.Presentation.Models;

namespace Jarvis.Application.Workflows.StartWorkflow;

public record StartWorkflowCommand(WorkflowName Workflow, Dictionary<string, object>? Input) : IRequest<Result<string>>;