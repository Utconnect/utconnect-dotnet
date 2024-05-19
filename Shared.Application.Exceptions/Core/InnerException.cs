namespace Shared.Application.Exceptions.Core;

public abstract class InnerException(string? message, Exception? innerException = null)
    : Exception(message, innerException)
{
    public abstract HttpException WrapException();
}