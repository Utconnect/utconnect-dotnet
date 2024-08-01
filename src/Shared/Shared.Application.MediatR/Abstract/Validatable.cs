using FluentValidation;
using FluentValidation.Results;
using Shared.Presentation.Models;

namespace Shared.Application.MediatR.Abstract;

public abstract class Validatable
{
    protected static async Task<Result> ValidateAsync<T>(
        IValidator<T> validator,
        T request,
        CancellationToken cancellationToken)
    {
        ValidationResult? result = await validator.ValidateAsync(request, cancellationToken);
        return result.IsValid
            ? Result.Succeed()
            : Result.FromFluentValidationFailures(result.Errors);
    }
}