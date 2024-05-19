namespace Shared.Application.Extensions;

public static class StringExtensions
{
    public static DateTime ToUnixDateTime(this string unixDate)
    {
        if (!long.TryParse(unixDate, out var longDate))
        {
            throw new Exception("Unix DateTime cannot be parsed, user cannot be identified.");
        }

        return DateTime.UnixEpoch.AddSeconds(longDate).ToLocalTime();
    }
}