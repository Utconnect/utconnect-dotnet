using Elsa.Workflows.Runtime.Contracts;
using Elsa.Workflows.Runtime.Parameters;
using Elsa.Workflows.Runtime.Results;
using FluentValidation;
using Jarvis.Domain.Workflows.User.AddNewTeacher;
using MediatR;
using Shared.Application.MediatR.Abstract;
using Shared.Presentation.Models;

namespace Jarvis.Application.Workflows.StartAddNewTeacherWorkflow;

internal class StartAddNewTeacherWorkflowCommandHandler(
    IWorkflowRuntime workflowRuntime,
    IValidator<StartAddNewTeacherWorkflowCommand> validator
)
    : Validatable, IRequestHandler<StartAddNewTeacherWorkflowCommand, Result<string>>
{
    public async Task<Result<string>> Handle(StartAddNewTeacherWorkflowCommand request, CancellationToken cancellationToken)
    {
        Result validateResult = await ValidateAsync(validator, request, cancellationToken);
        if (!validateResult.Success)
        {
            return Result<string>.Failure(validateResult.Errors!);
        }

        Dictionary<string, object> input = new()
        {
            { "Name", request.Name },
            { "DepartmentId", request.DepartmentId ?? Guid.Empty },
            { "FacultyId", request.FacultyId ?? Guid.Empty },
            { "Email", request.Email ?? string.Empty }
        };

        WorkflowExecutionResult result = await workflowRuntime.StartWorkflowAsync(
            AddNewTeacherWorkflow.DefinitionId, new StartWorkflowRuntimeParams
                { Input = input });

        return Result<string>.Succeed(result.WorkflowInstanceId);
    }
}