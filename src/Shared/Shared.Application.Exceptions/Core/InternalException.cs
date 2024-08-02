using System.Net;
using Shared.Application.Exceptions.Models;

namespace Shared.Application.Exceptions.Core;

public abstract class InternalServerErrorException(string? message, Exception? innerException = null)
    : InnerException(message, innerException)
{
    public override HttpException WrapException()
    {
        List<Error> errorResponse = [new InternalServerError(Message)];
        return new HttpException(HttpStatusCode.InternalServerError, errorResponse, Message, this);
    }
}