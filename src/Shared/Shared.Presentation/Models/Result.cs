using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Shared.Application.Exceptions.Models;

namespace Shared.Presentation.Models;

public class Result<T>
{
    public T? Data { get; set; }
    public bool Success { get; set; }
    public IEnumerable<Error>? Errors { get; set; }

    public static Result<T> Succeed(T data)
    {
        return new Result<T>
        {
            Success = true,
            Data = data
        };
    }

    public static Result<T> Failure(Error error)
    {
        return new Result<T>
        {
            Data = default,
            Success = false,
            Errors = new List<Error> { error }
        };
    }

    public static Result<T> Failure(IEnumerable<Error> errors)
    {
        return new Result<T>
        {
            Data = default,
            Success = false,
            Errors = errors
        };
    }

    public static Result<T> FromIdentityErrors(IEnumerable<IdentityError> errors)
    {
        return new Result<T>
        {
            Data = default,
            Success = false,
            Errors = errors.Select(e => new MessageError(e.Description))
        };
    }
}

public class Result : Result<bool>
{
    public static Result Succeed()
    {
        return new Result
        {
            Success = true,
            Data = true
        };
    }

    public new static Result Failure(Error error)
    {
        return new Result
        {
            Data = false,
            Success = false,
            Errors = new List<Error> { error }
        };
    }

    public static Result FromFluentValidationFailures(List<ValidationFailure> failures)
    {
        return new Result
        {
            Data = false,
            Success = false,
            Errors = failures.Select(f => new ValidationError(f.PropertyName, f.ErrorMessage))
        };
    }

    public new static Result FromIdentityErrors(IEnumerable<IdentityError> errors)
    {
        return new Result
        {
            Data = false,
            Success = false,
            Errors = errors.Select(e => new MessageError(e.Description))
        };
    }
}