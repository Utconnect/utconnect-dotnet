using Friday.Application.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Utconnect.Common.Models;

namespace Friday.Application.Workflows.UploadTeachingSchedule;

public record UploadTeachingScheduleCommand(
    IFormFile File,
    string AcademicYear,
    TrainingType TrainingType)
    : IRequest<Result<string>>;