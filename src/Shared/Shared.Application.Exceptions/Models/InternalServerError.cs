using System.Net;

namespace Shared.Application.Exceptions.Models;

public class InternalServerError(string message = "Server error")
    : Error(HttpStatusCode.InternalServerError, message);