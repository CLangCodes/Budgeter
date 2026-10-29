using SharedKernel;

namespace Domain.Models;

public enum RecurrenceUnit { Day, Week, Month, Year }

public class Expense : Entity
{
    public Guid ExpenseId { get; set; }
    public Guid CategoryId { get; set; }
    public string ExpenseName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public RecurrenceUnit RecurrenceUnit { get; set; }   // Month
    public int RecurrenceInterval { get; set; } = 1;     // every 1 month (2 = bi-monthly, 3 = quarterly)
    public DateOnly StartTrackingDate { get; set; }      // anchor: first due date, e.g. 2026-11-15
    public DateOnly? EndDate { get; set; }               // optional: loan payoff, cancelled subscription

    public IEnumerable<DateOnly> OccurrencesBetween(DateOnly from, DateOnly to)
    {
        for (int i = 0; ; i++)
        {
            DateOnly due = RecurrenceUnit switch
            {
                RecurrenceUnit.Day   => StartTrackingDate.AddDays(i * RecurrenceInterval),
                RecurrenceUnit.Week  => StartTrackingDate.AddDays(i * RecurrenceInterval * 7),
                RecurrenceUnit.Month => StartTrackingDate.AddMonths(i * RecurrenceInterval),
                RecurrenceUnit.Year  => StartTrackingDate.AddYears(i * RecurrenceInterval),
                _ => throw new InvalidOperationException()
            };
            if (due > to || (EndDate is { } end && due > end)) yield break;
            if (due >= from) yield return due;
        }
    }
}
