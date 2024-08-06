using Friday.Application.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Shared.Presentation.Models;

namespace Friday.Application.Workflows.StartAddUploadFileAndProcessWorkflow;

public record StartAddUploadFileAndProcessWorkflowCommand(
    IFormFile File,
    string AcademicYear,
    TrainingType TrainingType)
    : IRequest<Result<string>>;