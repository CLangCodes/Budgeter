namespace BudgetBuddy.Kernel;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}

internal sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
