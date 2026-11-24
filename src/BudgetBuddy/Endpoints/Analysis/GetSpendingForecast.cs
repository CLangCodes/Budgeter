using Application.Abstractions.Authorization;
using Application.Abstractions.Messaging;
using Application.Analysis;
using BudgetBuddy.Extensions;
using BudgetBuddy.Http;
using SharedKernel;

namespace BudgetBuddy.Endpoints.Analysis;

internal sealed class GetSpendingForecastEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/v1/analysis/{accountId:Guid}/spending-forecast", async (
        Guid accountId,
        DateOnly? startDate,
        DateOnly? endDate,
        IDateTimeProvider dateTimeProvider,
        IQueryHandler<GetSpendingForecastQuery, GetSpendingForecastResult> handler,
        CancellationToken cancellationToken) =>
        {
            var end = endDate ?? dateTimeProvider.UtcNowDateOnly;
            var start = startDate ?? end.AddDays(-30);

            Result<GetSpendingForecastResult> result = await handler.Handle(
                new GetSpendingForecastQuery(accountId, start, end), cancellationToken).ConfigureAwait(false);
            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .HasPermission(Permissions.AnalysisRead)
        .WithTags(Tags.Analysis)
        .Produces<GetSpendingForecastResult>()
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}
