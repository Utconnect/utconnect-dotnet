using MediatR;
using Utconnect.Common.Models;

namespace IdentityProvider.Application.User.Commands.AddUserToRole;

public record AddUserToRoleCommand(Guid UserId, List<string> RolesName) : IRequest<Result>;