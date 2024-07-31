using MediatR;
using Shared.Presentation.Models;

namespace Jarvis.Application.Workflows.StartAddNewTeacherWorkflow;

public record StartAddNewTeacherWorkflowCommand(
    string Name,
    Guid? DepartmentId,
    Guid? FacultyId,
    string? Email) : IRequest<Result<string>>;