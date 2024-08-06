using IdentityProvider.Application.User.Commands.AddUserToRole;
using IdentityProvider.Application.User.Commands.CreateUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Utconnect.Common.Models;

namespace IdentityProvider.Controllers;

[Route("api/[controller]")]
public class UserController(ISender mediatr) : Controller
{
    [HttpPost]
    public async Task<Result<CreateUserCommandResponse>> Create([FromBody] CreateUserCommand command)
    {
        Result<CreateUserCommandResponse> result = await mediatr.Send(command);
        return result;
    }

    [HttpPatch("{userId:guid}/role")]
    public async Task<Result> AddUserToRole(Guid userId, [FromBody] AddUserToRoleRequest request)
    {
        AddUserToRoleCommand command = new(userId, request.Roles);
        Result result = await mediatr.Send(command);
        return result;
    }
}