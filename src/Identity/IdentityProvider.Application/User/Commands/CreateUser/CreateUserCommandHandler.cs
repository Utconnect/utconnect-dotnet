using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Utconnect.Common.Helpers.Abstractions;
using Utconnect.Common.MediatR.Abstractions;
using Utconnect.Common.Models;

namespace IdentityProvider.Application.User.Commands.CreateUser;

internal class CreateUserCommandHandler(
    UserManager<Domain.Models.User> userManager,
    IStringHelper stringHelper,
    IValidator<CreateUserCommand> validator,
    ILogger<CreateUserCommandHandler> logger)
    : Validatable, IRequestHandler<CreateUserCommand, Result<CreateUserCommandResponse>>
{
    public async Task<Result<CreateUserCommandResponse>> Handle(
        CreateUserCommand request,
        CancellationToken cancellationToken)
    {
        Result validateResult = await ValidateAsync(validator, request, cancellationToken);
        if (!validateResult.Success)
        {
            return Result<CreateUserCommandResponse>.Failure(validateResult.Errors!);
        }

        string name = request.Name;
        string userName = string.IsNullOrEmpty(request.UserName) ? GenerateUserName(request.Name) : request.UserName;
        Domain.Models.User user = new(userName, name);

        IdentityResult createResult = await userManager.CreateAsync(user);
        if (!createResult.Succeeded)
        {
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

            return Result<CreateUserCommandResponse>.FromIdentityErrors(createResult.Errors);
        }

        Domain.Models.User createdUser = (await userManager.FindByNameAsync(userName))!;
        CreateUserCommandResponse responseData = new()
        {
            UserName = userName,
            Id = createdUser.Id
        };
        return Result.Succeed(responseData);
    }

    private string GenerateUserName(string name)
    {
        string[] wordsInName = stringHelper.RemoveDiacritics(name).Split(' ');
        string nickname = wordsInName.Length == 0
            ? wordsInName[0].ToLower()
            : (wordsInName[0] + '.' + wordsInName[^1]).ToLower();

        List<string> usedEquivalentUserName = userManager.Users
            .Select(u => u.UserName)
            .Where(u => !string.IsNullOrEmpty(u) && u.StartsWith(nickname))
            .ToList()!;

        if (usedEquivalentUserName.Count == 0 || !usedEquivalentUserName.Contains(nickname))
        {
            return nickname;
        }

        var count = 2;
        string userName;
        do
        {
            userName = nickname + count;
            count++;
        } while (usedEquivalentUserName.Contains(userName));

        return userName;
    }
}