using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Shared.Application.Exceptions.Core;
using Shared.Application.Exceptions.Models;
using Shared.Presentation.Models;

namespace Shared.Presentation.Filters;

public class HttpResponseExceptionFilter(IHostEnvironment hostEnvironment) : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        Exception exception = context.Exception;
        HttpException exceptionToHandle;

        switch (exception)
        {
            case HttpException httpException:
                exceptionToHandle = httpException;
                break;
            case InnerException innerException:
                exceptionToHandle = innerException.WrapException();
                break;
            default:
            {
                List<Error> errorResponse = [new Error(HttpStatusCode.InternalServerError, exception.Message)];
                string message = hostEnvironment.IsEnvironment(Environments.Development)
                    ? exception.Message
                    : "Server error";
                exceptionToHandle =
                    new HttpException(HttpStatusCode.InternalServerError, errorResponse, message, exception);
                break;
            }
        }

        Result<bool> response = new()
        {
            Success = false,
            Errors = exceptionToHandle.Errors
        };

        context.Result = new JsonResult(response)
        {
            StatusCode = (int?)exceptionToHandle.StatusCode
        };
        context.ExceptionHandled = true;
    }
}