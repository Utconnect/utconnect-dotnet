using FluentValidation;
using Jarvis.Common.Enums;
using Jarvis.Common.Services.Abstract;
using MediatR;
using Pince.Services.Abstract;
using Utconnect.Common.MediatR.Abstractions;
using Utconnect.Common.Models;
using Utconnect.Common.Models.Errors;

namespace Friday.Application.Workflows.UploadTeachingSchedule;

internal class UploadTeachingScheduleCommandHandler(
    IFileManagerService fileManagerService,
    IFileProcessorService fileProcessorService,
    IValidator<UploadTeachingScheduleCommand> validator
)
    : Validatable, IRequestHandler<UploadTeachingScheduleCommand, Result<string>>
{
    public async Task<Result<string>> Handle(
        UploadTeachingScheduleCommand request,
        CancellationToken cancellationToken)
    {
        Result validateResult = await ValidateAsync(validator, request, cancellationToken);
        if (!validateResult.Success)
        {
            return Result<string>.Failure(validateResult.Errors!);
        }

        string? uploadFilePath = await fileManagerService.Upload(cancellationToken); 
        if (uploadFilePath == null)
        {
            return Result<string>.Failure(new InternalServerError("Cannot upload file"));
        }

        Dictionary<string, object> input = new()
        {
            { "FilePath", uploadFilePath },
            { "AcademicYear", request.AcademicYear },
            { "TrainingType", request.TrainingType }
        };

        Result<string> result = await fileProcessorService.StartWorkflow(WorkflowName.UploadTeachingScheduleFile, input, cancellationToken);

        return result;
    }
}