using SharedKernel;

namespace Domain.Models;

public enum RecurrenceUnit { Day, Week, Month, Year }

public class BudgetItem : Entity
{
    public Guid BudgetItemId { get; private set; }
    public Guid CategoryId { get; private set; }
    public Category? Category { get; private set; }
    public Guid? AccountId { get; private set; }
    public string BudgetItemName { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;

    public RecurrenceUnit RecurrenceUnit { get; private set; }   // Month
    public int RecurrenceInterval { get; private set; } = 1;     // every 1 month (2 = bi-monthly, 3 = quarterly)
    public DateOnly StartTrackingDate { get; private set; }      // anchor: first due date, e.g. 2026-11-15
    public DateOnly? EndDate { get; private set; }               // optional: loan payoff, cancelled subscription
    public decimal Amount { get; private set; }                  // Debit value; positive is a withdrawal, negative is a deposit (e.g. refund)

    public static Result<BudgetItem> Create(
        Guid categoryId,
        Guid? accountId,
        string budgetItemName,
        string description,
        RecurrenceUnit recurrenceUnit,
        int recurrenceInterval,
        DateOnly startTrackingDate,
        DateOnly? endDate,
        decimal amount)
    {
        Result recurrence = ValidateRecurrence(recurrenceInterval, startTrackingDate, endDate);
        if (recurrence.IsFailure)
        {
            return Result.Failure<BudgetItem>(recurrence.Error);
        }

        return new BudgetItem
        {
            BudgetItemId = Guid.NewGuid(),
            CategoryId = categoryId,
            AccountId = accountId,
            BudgetItemName = budgetItemName,
            Description = description,
            RecurrenceUnit = recurrenceUnit,
            RecurrenceInterval = recurrenceInterval,
            StartTrackingDate = startTrackingDate,
            EndDate = endDate,
            Amount = amount
        };
    }

    public Result Update(
        Guid categoryId,
        Guid? accountId,
        string budgetItemName,
        string description,
        RecurrenceUnit recurrenceUnit,
        int recurrenceInterval,
        DateOnly startTrackingDate,
        DateOnly? endDate,
        decimal amount)
    {
        Result recurrence = ValidateRecurrence(recurrenceInterval, startTrackingDate, endDate);
        if (recurrence.IsFailure)
        {
            return recurrence;
        }

        CategoryId = categoryId;
        AccountId = accountId;
        BudgetItemName = budgetItemName;
        Description = description;
        RecurrenceUnit = recurrenceUnit;
        RecurrenceInterval = recurrenceInterval;
        StartTrackingDate = startTrackingDate;
        EndDate = endDate;
        Amount = amount;
        return Result.Success();
    }

    private static Result ValidateRecurrence(int recurrenceInterval, DateOnly startTrackingDate, DateOnly? endDate)
    {
        if (recurrenceInterval < 1)
            {return Result.Failure(BudgetItemErrors.InvalidRecurrence("RecurrenceInterval must be at least 1."));}
        if (endDate is { } end && end < startTrackingDate)
            {return Result.Failure(BudgetItemErrors.InvalidRecurrence("EndDate cannot be before StartTrackingDate."));}
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

public static class BudgetItemErrors
{
    public static Error NotFound(Guid budgetItemId) => Error.NotFound(
        "BudgetItems.NotFound",
        $"The budget item with the Id = '{budgetItemId}' was not found");
    public static Error InvalidRecurrence(string reason) => Error.Problem(
        "BudgetItems.InvalidRecurrence",
        reason);
}
