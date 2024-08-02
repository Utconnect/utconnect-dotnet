using System.Net;

namespace Shared.Application.Exceptions.Models;

public class BadRequestError(string message = "This request cannot be handled")
    : Error(HttpStatusCode.BadRequest, message);