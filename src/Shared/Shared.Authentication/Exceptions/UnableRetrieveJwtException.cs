namespace Shared.Authentication.Exceptions;

public class UnableRetrieveJwtException(string message) : Exception($"Failed to retrieve JWT key from API: {message}");