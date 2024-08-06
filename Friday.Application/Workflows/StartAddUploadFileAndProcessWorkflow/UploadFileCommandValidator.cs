using FluentValidation;

namespace Friday.Application.Workflows.StartAddUploadFileAndProcessWorkflow;

public class StartAddUploadFileAndProcessWorkflowCommandValidator : AbstractValidator<StartAddUploadFileAndProcessWorkflowCommand>
{
    public StartAddUploadFileAndProcessWorkflowCommandValidator()
    {
        RuleFor(e => e.File).NotEmpty();
        RuleFor(e => e.AcademicYear).NotEmpty();
        RuleFor(e => e.TrainingType).NotEmpty().IsInEnum();
    }
}