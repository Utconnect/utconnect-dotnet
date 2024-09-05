using FluentValidation;

namespace IdentityProvider.Application.Users.Commands.CreateUser;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(e => e.Name).NotEmpty();
    }
}