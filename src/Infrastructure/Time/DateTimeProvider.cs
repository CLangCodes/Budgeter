using SharedKernel;

namespace Infrastructure.Time;

internal sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;

    public static (DateOnly Sunday, DateOnly Saturday) GetWeekBounds(
        DateOnly date)
    {
        var sunday = date.AddDays(-(int)date.DayOfWeek);
        return (sunday, sunday.AddDays(6));
    }
    public static (DateTime Sunday, DateTime Saturday) GetWeekBounds(
        DateTime date)
    {
        var start = date.Date.AddDays(-(int)date.DayOfWeek);
        var end = start.AddDays(7);
        return (start, end);
    }
    public static (DateOnly FirstDay, DateOnly LastDay) GetMonthBounds(
        DateOnly date)
    {
        var firstDay = new DateOnly(date.Year, date.Month, 1);
        var lastDay = firstDay.AddMonths(1).AddDays(-1);
        return (firstDay, lastDay);
    }
    public static (DateTime FirstDay, DateTime LastDay) GetMonthBounds(
        DateTime date)
    {
        var firstDay = new DateTime(date.Year, date.Month, 1);
        var lastDay = firstDay.AddMonths(1).AddDays(-1);
        return (firstDay, lastDay);
    }
}
