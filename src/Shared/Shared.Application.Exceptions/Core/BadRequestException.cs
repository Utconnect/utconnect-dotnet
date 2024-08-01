using System.Net;
using Shared.Application.Exceptions.Models;

namespace Shared.Application.Exceptions.Core;

public abstract class BadRequestException(string? message, Exception? innerException = null)
    : InnerException(message, innerException)
{
    protected const HttpStatusCode Code = HttpStatusCode.BadRequest;

    public override HttpException WrapException()
    {
        var errorResponse = new List<Error> { new(Code, Message) };
        return new HttpException(Code, errorResponse, Message, this);
    }
}