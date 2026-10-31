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

    public static Expense Create(
        Guid categoryId,
        string expenseName,
        string description,
        RecurrenceUnit recurrenceUnit,
        int recurrenceInterval,
        DateOnly startTrackingDate,
        DateOnly? endDate
    )
        {
            return new Expense
            {
                ExpenseId = Guid.NewGuid(),
                CategoryId = categoryId,
                ExpenseName = expenseName,
                Description = description,
                RecurrenceUnit = recurrenceUnit,
                RecurrenceInterval = recurrenceInterval,
                StartTrackingDate = startTrackingDate,
                EndDate = endDate
            };
        }

    public void Update(
        Guid categoryId,
        string expenseName,
        string description,
        RecurrenceUnit recurrenceUnit,
        int recurrenceInterval,
        DateOnly startTrackingDate,
        DateOnly? endDate)
    {
        CategoryId = categoryId;
        ExpenseName = expenseName;
        Description = description;
        RecurrenceUnit = recurrenceUnit;
        RecurrenceInterval = recurrenceInterval;
        StartTrackingDate = startTrackingDate;
        EndDate = endDate;
    }

    public static Result ValidateRecurrence(int recurrenceInterval, DateOnly startTrackingDate, DateOnly? endDate)
    {
        if (recurrenceInterval < 1)
            {return Result.Failure(ExpenseErrors.InvalidRecurrence("RecurrenceInterval must be at least 1."));}
        if (endDate is { } end && end < startTrackingDate)
            {return Result.Failure(ExpenseErrors.InvalidRecurrence("EndDate cannot be before StartTrackingDate."));}
        return Result.Success();
    }

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
public static class ExpenseErrors
{
    public static Error NotFound(Guid expenseId) => Error.NotFound(
        "Expenses.NotFound",
        $"The expense with the Id = '{expenseId}' was not found");
    public static Error InvalidRecurrence(string reason) => Error.Problem(
        "Expenses.InvalidRecurrence",
        reason);
}
