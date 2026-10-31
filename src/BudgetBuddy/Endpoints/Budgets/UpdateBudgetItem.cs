using Application.Abstractions.Messaging;
using Application.Budgets;
using BudgetBuddy.Extensions;
using BudgetBuddy.Infrastructure;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace BudgetBuddy.Endpoints.Budgets;

internal sealed class UpdateBudgetItemEndpoint : IEndpoint
{
    internal sealed record UpdateBudgetItemRequest(
        Guid CategoryId,
        string BudgetItemName,
        string Description,
        RecurrenceUnit RecurrenceUnit,
        int RecurrenceInterval,
        DateOnly StartTrackingDate,
        DateOnly? EndDate);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/v1/budgets/{budgetItemId:Guid}", async (
        Guid budgetItemId,
        [FromBody]
        UpdateBudgetItemRequest request,
        ICommandHandler<UpdateBudgetItemCommand> handler,
        CancellationToken cancellationToken) =>
        {
            Result result = await handler.Handle(
                new UpdateBudgetItemCommand(
                    budgetItemId,
                    request.CategoryId,
                    request.BudgetItemName,
                    request.Description,
                    request.RecurrenceUnit,
                    request.RecurrenceInterval,
                    request.StartTrackingDate,
                    request.EndDate
                    ),
                cancellationToken).ConfigureAwait(false);
            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.Budgets)
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}
