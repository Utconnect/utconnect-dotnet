using System.Security.Claims;
using IdentityProvider.Application.Dtos;
using IdentityProvider.Application.Users.Commands.AddUserToRole;
using IdentityProvider.Application.Users.Commands.CreateUser;
using IdentityProvider.Application.Users.Queries.GetUserInfo;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Utconnect.Common.Models;

namespace IdentityProvider.Controllers;

[Route("api/[controller]")]
public class UserController(ISender sender) : Controller
{
    [HttpPost]
    public async Task<Result<CreateUserCommandResponse>> Create([FromBody] CreateUserCommand command)
    {
        Result<CreateUserCommandResponse> result = await sender.Send(command);
        return result;
    }

    [HttpGet("me")]
    public async Task<Result<UserInfoDto>> GetMyUserInfo()
    {
        GetUserInfoQuery query = new(User.FindFirstValue(ClaimTypes.NameIdentifier));
        Result<UserInfoDto> result = await sender.Send(query);
        return result;
    }

    [HttpGet("{userId}")]
    public async Task<Result<UserInfoDto>> GetUserInfo(string userId)
    {
        GetUserInfoQuery query = new(userId);
        Result<UserInfoDto> result = await sender.Send(query);
        return result;
    }

    [HttpPatch("{userId:guid}/role")]
    public async Task<Result> AddUserToRole(Guid userId, [FromBody] AddUserToRoleRequest request)
    {
        AddUserToRoleCommand command = new(userId, request.Roles);
        Result result = await sender.Send(command);
        return result;
    }
}