using System.Net;

namespace Shared.Application.Exceptions.Models;

public class NotFoundError(string message) : Error(HttpStatusCode.NotFound, message);

public class NotFoundError<T>(string value, string key = "ID")
    : NotFoundError($"{typeof(T).Name} with {key} {value} is not found");