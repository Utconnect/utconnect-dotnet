using FluentValidation;
using IdentityProvider.Application.Dtos;
using IdentityProvider.Application.Services;
using MediatR;
using Utconnect.Common.MediatR.Abstractions;
using Utconnect.Common.Models;

namespace IdentityProvider.Application.Users.Queries.GetUserInfo;

internal class GetUserInfoQueryHandler(UserService userService, IValidator<GetUserInfoQuery> validator)
    : Validatable, IRequestHandler<GetUserInfoQuery, Result<UserInfoDto>>
{
    public async Task<Result<UserInfoDto>> Handle(GetUserInfoQuery query, CancellationToken cancellationToken)
    {
        Result validateResult = await ValidateAsync(validator, query, cancellationToken);
        if (!validateResult.Success)
        {
            return Result<UserInfoDto>.Failure(validateResult.Errors!);
        }

        return await userService.GetUserByIdAsync(query.UserId!);
    }
}