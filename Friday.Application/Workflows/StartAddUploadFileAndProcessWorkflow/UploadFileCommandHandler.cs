using FluentValidation;
using Jarvis.Common.Enums;
using Jarvis.Common.Services.Abstract;
using MediatR;
using Shared.Application.MediatR.Abstract;
using Shared.Presentation.Models;

namespace Friday.Application.Workflows.StartAddUploadFileAndProcessWorkflow;

internal class StartAddUploadFileAndProcessWorkflowCommandHandler(
    IJarvisService jarvisService,
    IValidator<StartAddUploadFileAndProcessWorkflowCommand> validator
)
    : Validatable, IRequestHandler<StartAddUploadFileAndProcessWorkflowCommand, Result<string>>
{
    public async Task<Result<string>> Handle(StartAddUploadFileAndProcessWorkflowCommand request,
        CancellationToken cancellationToken)
    {
        Result validateResult = await ValidateAsync(validator, request, cancellationToken);
        if (!validateResult.Success)
        {
            return Result<string>.Failure(validateResult.Errors!);
        }

        Result<string> result = await jarvisService.StartWorkflow(WorkflowName.UploadTeachingScheduleFile,
            new Dictionary<string, object>(),
            cancellationToken);

        return result;
    }
}