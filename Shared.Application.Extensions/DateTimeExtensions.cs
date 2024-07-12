namespace Shared.Application.Extensions;

public static class DateTimeExtensions
{
    public static long ToTimestamp(this DateTime dateTime)
    {
        TimeSpan elapsedTime = dateTime - DateTime.UnixEpoch;
        return (long)elapsedTime.TotalSeconds;
    }
}