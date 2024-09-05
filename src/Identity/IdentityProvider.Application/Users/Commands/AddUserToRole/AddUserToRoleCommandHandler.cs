using FluentValidation;
using IdentityProvider.Domain.Constants;
using IdentityProvider.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Utconnect.Common.MediatR.Abstractions;
using Utconnect.Common.Models;
using Utconnect.Common.Models.Errors;

namespace IdentityProvider.Application.Users.Commands.AddUserToRole;

internal class AddUserToRoleCommandHandler(
    UserManager<User> userManager,
    IValidator<AddUserToRoleCommand> validator,
    ILogger<AddUserToRoleCommandHandler> logger)
    : Validatable, IRequestHandler<AddUserToRoleCommand, Result>
{
    private readonly List<string> _allowedToGrantRoles = [RoleConstant.Teacher];

    public async Task<Result> Handle(AddUserToRoleCommand request, CancellationToken cancellationToken)
    {
        Result validateResult = await ValidateAsync(validator, request, cancellationToken);
        if (!validateResult.Success)
        {
            return validateResult;
        }

        Guid userId = request.UserId;
        List<string> rolesRequestedToGrant = request.RolesName;

        string? notAllowedRole = rolesRequestedToGrant.Find(role => !_allowedToGrantRoles.Contains(role));
        if (notAllowedRole != null)
        {
            return Result.Failure(new BadRequestError($"Role {notAllowedRole} is not allowed to grant"));
        }

        User? user = await userManager.FindByIdAsync(userId.ToString());
        if (user == null)
        {
            return Result.Failure(new NotFoundError<User>(userId.ToString()));
        }

        IList<string> userRoles = await userManager.GetRolesAsync(user);
        IEnumerable<string> rolesToGrant = rolesRequestedToGrant.Where(role => !userRoles.Contains(role));

        IdentityResult createResult = await userManager.AddToRolesAsync(user, rolesToGrant);
        if (createResult.Succeeded)
        {
            return Result.Succeed();
        }

        var errorIdx = 0;
        foreach (IdentityError error in createResult.Errors)
        {
            logger.LogError(
                "Creating user failed #{ErrorIdx} ({ErrorCode}): {ErrorDescription}",
                errorIdx,
                error.Code,
                error.Description);
            errorIdx++;
        }

        return Result.FromIdentityErrors(createResult.Errors);
    }
}