using System.Net;
using FluentValidation;
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
        var exception = context.Exception;
        HttpException exceptionToHandle;

        switch (exception)
        {
            case HttpException httpException:
                exceptionToHandle = httpException;
                break;
            case InnerException innerException:
                exceptionToHandle = innerException.WrapException();
                break;
            case ValidationException validationException:
            {
                IEnumerable<Error> errorResponse = validationException.Errors
                    .Select(e => new Error(e.PropertyName, e.ErrorMessage));
                exceptionToHandle = new HttpException(HttpStatusCode.BadRequest, errorResponse);
                break;
            }
            default:
            {
                List<Error> errorResponse = [new Error(HttpStatusCode.InternalServerError, exception.Message)];
                var message = hostEnvironment.IsEnvironment(Environments.Development)
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