using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Shared.Application.Exceptions.Models;
using Shared.Application.MediatR.Abstract;
using Shared.Helper;
using Shared.Presentation.Models;

namespace IdentityProvider.Application.User.Commands.CreateUser;

internal class CreateUserCommandHandler(
    UserManager<Domain.Models.User> userManager,
    StringHelper stringHelper,
    IValidator<CreateUserCommand> validator,
    ILogger<CreateUserCommandHandler> logger)
    : Validatable, IRequestHandler<CreateUserCommand, Result>
{
    public async Task<Result> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        Result validateResult = await ValidateAsync(validator, request, cancellationToken);
        if (!validateResult.Success)
        {
            return validateResult;
        }
        
        string name = request.Name;
        string userName = string.IsNullOrEmpty(request.UserName) ? GenerateUserName(request.Name) : request.UserName;
        Domain.Models.User user = new(userName, name);

        IdentityResult createResult = await userManager.CreateAsync(user);
        if (createResult.Succeeded)
        {
            return Result.Succeed();
        }

        int errorIdx = 0;
        foreach (IdentityError error in createResult.Errors)
        {
            logger.LogError(
                "Creating user failed #{ErrorIdx} ({ErrorCode}): {ErrorDescription}",
                errorIdx,
                error.Code,
                error.Description);
            errorIdx++;
        }

        return Result.Failure(new Error(""));
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

        int count = 2;
        string userName;
        do
        {
            userName = nickname + count;
            count++;
        } while (usedEquivalentUserName.Contains(userName));

        return userName;
    }
}