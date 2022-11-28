using System;

namespace GenshinWoodmen.Models;

public static class UpdateTime
{
    public static DateTime NextUpdateTime = UpdateNextTime();
    public static string NextUpdateTimeViewString => NextUpdateTime.ToString("yyyy/MM/dd HH:mm:ss");

    public static bool IsCountStartedFormNextUpdateTime { get; set; } = false;

    public static DateTime UpdateNextTime()
    {
        int timeZoneOffset8 = 8 - TimeZoneInfo.Local.BaseUtcOffset.Hours;
        DateTime dateTime8 = DateTime.Now.AddHours(timeZoneOffset8);
        DateTime dateTimeUpdate8 = new(dateTime8.Year, dateTime8.Month, dateTime8.Day, 4, 0, 0, DateTimeKind.Utc);
        
        if (dateTime8.Hour >= 4)
        {
            dateTimeUpdate8 = dateTimeUpdate8.AddDays(1);
        }
        return NextUpdateTime = dateTimeUpdate8.AddHours(-timeZoneOffset8);
    }
}
