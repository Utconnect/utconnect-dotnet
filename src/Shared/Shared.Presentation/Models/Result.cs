using FluentValidation.Results;
using Shared.Application.Exceptions.Models;

namespace Shared.Presentation.Models;

public class Result<T>
{
    public T Data { get; set; } = default!;
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

    public static Result<bool> Failure(Error error)
    {
        return new Result<bool>
        {
            Success = false,
            Data = false,
            Errors = new List<Error> { error }
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
            Success = false,
            Data = false,
            Errors = new List<Error> { error }
        };
    }

    public static Result FromFluentValidationFailures(List<ValidationFailure> failures)
    {
        return new Result
        {
            Success = false,
            Data = false,
            Errors = failures.Select(f => new ValidationError(f.PropertyName, f.ErrorMessage))
        };
    }
}