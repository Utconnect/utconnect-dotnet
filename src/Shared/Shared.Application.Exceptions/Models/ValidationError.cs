namespace Shared.Application.Exceptions.Models;

public class ValidationError(string property, string message) : Error(property, message);