using FluentValidation;
using Jarvis.Common.Enums;
using Jarvis.Common.Services.Abstract;
using MediatR;
using Shared.Application.MediatR.Abstract;
using Shared.Presentation.Models;

namespace Friday.Application.Files.UploadFile;

internal class UploadFileCommandHandler(IJarvisService jarvisService, IValidator<UploadFileCommand> validator)
    : Validatable, IRequestHandler<UploadFileCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UploadFileCommand request, CancellationToken cancellationToken)
    {
        Result validateResult = await ValidateAsync(validator, request, cancellationToken);
        if (!validateResult.Success)
        {
            return Result<string>.Failure(validateResult.Errors!);
        }
        
        Dictionary<string, object> input = new ()
        {
            {""}
        }

        jarvisService.StartWorkflow(WorkflowName.UploadTeachingScheduleFile, new Dictionary<string, object>(),
            cancellationToken);

        return Result<string>.Succeed(result.WorkflowInstanceId);
    }
}