using FluentValidation.Results;
using Shared.Application.Exceptions.Models;

namespace Shared.Application.Exceptions.Core;

public class UtconnectValidationException : BadRequestException
{
    public UtconnectValidationException(IEnumerable<ValidationFailure> failures)
        : base("One or more validation failures have occurred.")
    {
        Errors = failures
            .Select(f => new Error(f.PropertyName, f.ErrorMessage));
    }

    private IEnumerable<Error> Errors { get; }

    public override HttpException WrapException()
    {
        return new HttpException(Code, Errors, Message, this);
    }
}