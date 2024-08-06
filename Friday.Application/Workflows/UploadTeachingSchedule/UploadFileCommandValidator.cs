using FluentValidation;

namespace Friday.Application.Workflows.UploadTeachingSchedule;

public class UploadTeachingScheduleCommandValidator : AbstractValidator<UploadTeachingScheduleCommand>
{
    public UploadTeachingScheduleCommandValidator()
    {
        RuleFor(e => e.File).NotEmpty();
        RuleFor(e => e.AcademicYear).NotEmpty();
        RuleFor(e => e.TrainingType).NotEmpty().IsInEnum();
    }
}