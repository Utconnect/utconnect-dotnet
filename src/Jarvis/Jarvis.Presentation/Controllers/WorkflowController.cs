using Jarvis.Application.Workflows.StartAddNewTeacherWorkflow;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Presentation.Models;

namespace Jarvis.Server.Controllers;

[Route("[controller]")]
public class WorkflowController(ISender mediatr) : Controller
{
    [HttpPost("user/add-new-teacher")]
    public async Task<Result<string>> StartAddNewTeacherWorkflow([FromBody] StartAddNewTeacherWorkflowCommand command)
    {
        Result<string> result = await mediatr.Send(command);
        return result;
    }
}