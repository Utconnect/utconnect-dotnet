using Microsoft.Extensions.Logging;

namespace Shared.Logging;

public static class LoggerExtensions
{
    private static readonly Action<ILogger, string, Exception?> LoggerError;

    static LoggerExtensions()
    {
        LoggerError = LoggerMessage.Define<string>(LogLevel.Error, new EventId(5), "Error : {Message}");
    }

    public static void Error(this ILogger logger, string message) => LoggerError(logger, message, null);
}