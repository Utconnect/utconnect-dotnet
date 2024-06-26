using System.Net;
using JetBrains.Annotations;

namespace Shared.Application.Exceptions.Models;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class Error
{
    public HttpStatusCode? Code { get; }
    public string Message { get; }
    public string? Property { get; }

    public Error(string message)
    {
        Message = message;
    }

    public Error(HttpStatusCode code, string message)
    {
        Code = code;
        Message = message;
    }

    public Error(string property, string message)
    {
        Property = property;
        Message = message;
    }
}