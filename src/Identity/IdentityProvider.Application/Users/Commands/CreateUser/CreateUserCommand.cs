using MediatR;
using Utconnect.Common.Models;

namespace IdentityProvider.Application.Users.Commands.CreateUser;

public class CreateUserCommand : IRequest<Result<CreateUserCommandResponse>>
{
    public string? UserName { get; set; }
    public string Name { get; set; } = default!;
}