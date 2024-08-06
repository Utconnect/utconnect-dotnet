using Elsa.Workflows.Runtime.Contracts;
using Elsa.Workflows.Runtime.Parameters;
using Elsa.Workflows.Runtime.Results;
using FluentValidation;
using MediatR;
using Shared.Application.Extensions;
using Shared.Application.MediatR.Abstract;
using Shared.Presentation.Models;

namespace Jarvis.Application.Workflows.StartWorkflow;

internal class StartWorkflowCommandHandler(
    IWorkflowRuntime workflowRuntime,
    IValidator<StartWorkflowCommand> validator
)
    : Validatable, IRequestHandler<StartWorkflowCommand, Result<string>>
{
    public async Task<Result<string>> Handle(StartWorkflowCommand request, CancellationToken cancellationToken)
    {
        Result validateResult = await ValidateAsync(validator, request, cancellationToken);
        if (!validateResult.Success)
        {
            return Result<string>.Failure(validateResult.Errors!);
        }

        string workflowName = request.Workflow.GetDescription();
        StartWorkflowRuntimeParams startWorkflowRuntimeParams = new()
        {
            Input = request.Input
        };

        WorkflowExecutionResult result =
            await workflowRuntime.StartWorkflowAsync(workflowName, startWorkflowRuntimeParams);

        return Result<string>.Succeed(result.WorkflowInstanceId);
    }
}