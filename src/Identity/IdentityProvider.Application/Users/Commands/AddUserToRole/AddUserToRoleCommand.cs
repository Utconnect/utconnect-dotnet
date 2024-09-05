using MediatR;
using Utconnect.Common.Models;

namespace IdentityProvider.Application.Users.Commands.AddUserToRole;

public record AddUserToRoleCommand(Guid UserId, List<string> RolesName) : IRequest<Result>;