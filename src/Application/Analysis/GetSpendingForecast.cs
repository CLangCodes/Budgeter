using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Models;
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
        // Load the transactions so the domain's Account.Balance rule does the calculation.
        // (Projecting a.Balance in SQL silently drops the Include, and SQLite cannot Sum decimals.)
        Account? account = await context.Accounts
            .AsNoTracking()
            .Include(a => a.Transactions)
            .SingleOrDefaultAsync(a => a.AccountId == query.AccountId, cancellationToken);

        if (account is null)
        {
            return Result.Failure<GetSpendingForecastResult>(AccountErrors.NotFound(query.AccountId));
        }

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

        decimal balance = account.Balance;
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