using Jarvis.Application.Workflows.StartWorkflow;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Utconnect.Common.Models;

namespace Jarvis.Server.Controllers;

[Route("[controller]")]
public class WorkflowController(ISender mediatr) : Controller
{
    [HttpPost("start")]
    public async Task<Result<string>> StartAddNewTeacherWorkflow([FromBody] StartWorkflowCommand command)
    {
        Result<string> result = await mediatr.Send(command);
        return result;
    }
}