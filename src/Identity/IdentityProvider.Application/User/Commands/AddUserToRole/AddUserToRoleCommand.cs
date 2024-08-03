using MediatR;
using Shared.Presentation.Models;

namespace IdentityProvider.Application.User.Commands.AddUserToRole;

public record AddUserToRoleCommand(Guid UserId, List<string> RolesName) : IRequest<Result>;