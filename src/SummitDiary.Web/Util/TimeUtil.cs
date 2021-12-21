namespace SummitDiary.Web.Util;

public static class TimeUtil
{
    public static DateTime? ParseTime(this string time, DateTime date)
    {
        var parts = time.Split(":");
        if (parts.Length != 2)
            return null;

        if (!int.TryParse(parts[0], out var hour))
            return null;
        if (!int.TryParse(parts[1], out var minute))
            return null;
        return new DateTime(date.Year, date.Month, date.Day, hour, minute, 0);
    }
}