namespace Shared.Application.Extensions;

public static class StringExtensions
{
    public static DateTime? ToUnixDateTime(this string unixDate)
    {
        if (!long.TryParse(unixDate, out long longDate))
        {
            return null;
        }

        return DateTime.UnixEpoch.AddSeconds(longDate).ToLocalTime();
    }
}