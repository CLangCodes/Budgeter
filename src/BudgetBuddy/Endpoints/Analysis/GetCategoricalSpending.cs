using Application.Abstractions.Authorization;
using Application.Abstractions.Messaging;
using Application.Analysis;
using BudgetBuddy.Extensions;
using BudgetBuddy.Http;
using SharedKernel;

namespace BudgetBuddy.Endpoints.Analysis;

internal sealed class GetCategoricalSpendingEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/v1/analysis/{accountId:Guid}/categorical-spending", async (
        Guid accountId,
        DateOnly? startDate,
        DateOnly? endDate,
        IDateTimeProvider dateTimeProvider,
        IQueryHandler<GetCategoricalSpendingQuery, CategoricalSpendingResult> handler,
        CancellationToken cancellationToken) =>
        {
            var end = endDate ?? dateTimeProvider.UtcNowDateOnly;
            var start = startDate ?? end.AddDays(-30);

            Result<CategoricalSpendingResult> result = await handler.Handle(
                new GetCategoricalSpendingQuery(accountId, start, end), cancellationToken).ConfigureAwait(false);
            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .HasPermission(Permissions.AnalysisRead)
        .WithTags(Tags.Analysis)
        .Produces<CategoricalSpendingResult>();
    }
}
