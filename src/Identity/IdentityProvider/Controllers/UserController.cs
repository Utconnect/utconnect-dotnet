using IdentityProvider.Application.User.Commands.CreateUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Presentation.Models;

namespace IdentityProvider.Controllers;

[Route("api/[controller]")]
public class UserController(ISender mediatr) : Controller
{
    [HttpPost]
    public async Task<Result<CreateUserCommandResponse>> Create([FromBody] CreateUserCommand command)
    {
        Result<CreateUserCommandResponse> result = await mediatr.Send(command);
        Result result = await mediatr.Send(command);
        return result;
    }
}