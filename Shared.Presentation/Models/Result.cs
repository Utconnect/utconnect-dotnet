using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Shared.Application.Exceptions.Models;

namespace Shared.Presentation.Models;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class Result<T>
{
    public T Data { get; set; } = default!;
    public bool Success { get; set; }
    public IEnumerable<Error>? Errors { get; set; }

    public static Result<T> Get(T data)
    {
        return new Result<T>
        {
            Success = true,
            Data = data
        };
    }

    public static Result<bool> Failure(IEnumerable<IdentityError> errors)
    {
        return new Result<bool>
        {
            Success = false,
            Data = false,
            Errors = errors.Select(e => new Error(e.Code, e.Description))
        };
    }
}

public abstract class Result : Result<bool>;