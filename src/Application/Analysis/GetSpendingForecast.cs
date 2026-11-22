using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Analysis;

public sealed record GetSpendingForecastQuery(
    Guid AccountId, DateOnly StartDate, 
    DateOnly EndDate) : IQuery<GetSpendingForecastResult>;
public sealed record SpendingForecastResult(
    DateOnly ForecastDate, 
    decimal ForecastedSpending,
    decimal NewBalance,
    string? Description = null,
    string? CategoryName = null,
    string? CategoryDescription = null);
public sealed record GetSpendingForecastResult(
    List<SpendingForecastResult> Forecasts);

internal sealed class GetSpendingForecastQueryHandler(
    IApplicationDbContext context, IDateTimeProvider dateTimeProvider) : IQueryHandler<
        GetSpendingForecastQuery, 
        GetSpendingForecastResult>
{
    public async Task<Result<GetSpendingForecastResult>> Handle(
        GetSpendingForecastQuery query, 
        CancellationToken cancellationToken)
    {
        List<SpendingForecastResult> forecasts = new List<SpendingForecastResult>();
        decimal accountBalance = await context.Accounts
            .AsNoTracking()
            .Where(a => a.AccountId == query.AccountId)
            .Include(account => account.Transactions)
            .Select(a => a.Balance)
            .SingleOrDefaultAsync(cancellationToken);

        var budgetItems = await context.BudgetItems
            .AsNoTracking()
            .Where( b => b.AccountId == query.AccountId)
            .Include(b => b.Category)
            .ToListAsync(cancellationToken);

        var occurrences = budgetItems
            .SelectMany(b => b.OccurrencesBetween(query.StartDate, query.EndDate),
                        (b, due) => (
                            Date: due, 
                            b.Amount, 
                            b.Description, 
                            b.Category!.CategoryName ?? string.Empty,
                            b.Category.Description ?? string.Empty
                            ))
            .OrderBy(o => o.Date);

        var balance = accountBalance;
        forecasts.Add(new SpendingForecastResult(
            DateOnly.FromDateTime(dateTimeProvider.UtcNow), 
            0m, 
            balance, 
            "Starting Balance"));

        foreach (var (date, amount, description, categoryName, categoryDescription) in occurrences)
        {
            balance -= amount;
            forecasts.Add(new SpendingForecastResult(
                date,
                amount,
                balance,
                description,
                categoryName,
                categoryDescription
            ));
        }

        return new GetSpendingForecastResult(forecasts);
    }
}