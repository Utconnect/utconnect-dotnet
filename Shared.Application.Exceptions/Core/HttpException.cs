using System.Net;
using Shared.Application.Exceptions.Models;

namespace Shared.Application.Exceptions.Core;

[Serializable]
public class HttpException(
    HttpStatusCode statusCode,
    IEnumerable<Error> errors,
    string message = "",
    Exception? innerException = null)
    : Exception(message, innerException)
{
    public HttpStatusCode StatusCode { get; } = statusCode;

    public IEnumerable<Error> Errors { get; } = errors;
}