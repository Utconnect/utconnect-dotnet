using FluentValidation;

namespace Jarvis.Application.Workflows.StartAddNewTeacherWorkflow;

public class StartAddNewTeacherWorkflowCommandValidator : AbstractValidator<StartAddNewTeacherWorkflowCommand>
{
    public StartAddNewTeacherWorkflowCommandValidator()
    {
        RuleFor(e => e.Name).NotEmpty();
        RuleFor(e => e.Email).EmailAddress();
    }
}