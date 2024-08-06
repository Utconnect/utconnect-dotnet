using Friday.Application.Workflows.UploadTeachingSchedule;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Utconnect.Common.Models;

namespace Friday.Presentation.Controllers;

[Route("file-processor")]
public class FileProcessorController(ISender mediatr) : Controller
{
    [HttpPost("teaching-schedule")]
    public async Task<Result<string>> UploadTeachingSchedule([FromBody] UploadTeachingScheduleCommand command)
    {
        Result<string> result = await mediatr.Send(command);
        return result;
    }
}