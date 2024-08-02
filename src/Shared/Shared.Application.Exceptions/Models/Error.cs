using System.Net;

namespace Shared.Application.Exceptions.Models;

public abstract class Error
{
    public HttpStatusCode? Code { get; }
    public string Message { get; }
    public string? Property { get; }

    internal Error(HttpStatusCode code, string message)
    {
        Code = code;
        Message = message;
    }

    protected Error(string property, string message)
    {
        Property = property;
        Message = message;
    }
}