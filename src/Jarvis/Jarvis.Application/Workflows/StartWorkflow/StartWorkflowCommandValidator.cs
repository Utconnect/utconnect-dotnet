using FluentValidation;

namespace Jarvis.Application.Workflows.StartWorkflow;

public class StartWorkflowCommandValidator : AbstractValidator<StartWorkflowCommand>
{
    public StartWorkflowCommandValidator()
    {
        RuleFor(e => e.Workflow).IsInEnum();
    }
}