namespace Shared.Application.Extensions;

public static class DateTimeExtensions
{
    private static readonly DateTime Epoch = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public static long ToTimestamp(this DateTime dateTime)
    {
        TimeSpan elapsedTime = dateTime - Epoch;
        return (long)elapsedTime.TotalSeconds;
    }
}