using FluentValidation;

namespace IdentityProvider.Application.Users.Queries.GetUserInfo;

public class GetUserInfoQueryValidator : AbstractValidator<GetUserInfoQuery>
{
    public GetUserInfoQueryValidator()
    {
        RuleFor(e => e.UserId).NotEmpty();
        RuleFor(e => e.UserId).Must(id => Guid.TryParse(id, out _))
            .WithMessage("UserId should be a Guid");
    }
}