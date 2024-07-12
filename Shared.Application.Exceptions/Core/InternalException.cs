using System.Net;
using Shared.Application.Exceptions.Models;

namespace Shared.Application.Exceptions.Core;

public abstract class InternalServerErrorException(string? message, Exception? innerException = null)
    : InnerException(message, innerException)
{
    private const HttpStatusCode Code = HttpStatusCode.InternalServerError;

    public override HttpException WrapException()
    {
        List<Error> errorResponse = [new Error(Code, Message)];
        return new HttpException(Code, errorResponse, Message, this);
    }
}