using FluentValidation;

namespace IdentityProvider.Application.User.Commands.AddUserToRole;

public class AddUserToRoleCommandValidator : AbstractValidator<AddUserToRoleCommand>
{
    public AddUserToRoleCommandValidator()
    {
        RuleFor(e => e.UserId).NotEmpty();
        RuleFor(e => e.RolesName).NotEmpty();
        RuleForEach(e => e.RolesName).NotEmpty();
    }
}