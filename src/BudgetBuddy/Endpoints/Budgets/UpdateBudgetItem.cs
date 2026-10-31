using Application.Abstractions.Messaging;
using Application.Expenses;
using BudgetBuddy.Extensions;
using BudgetBuddy.Infrastructure;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace BudgetBuddy.Endpoints.Expenses;

internal sealed class UpdateExpenseEndpoint : IEndpoint
{
    internal sealed record UpdateExpenseRequest(
        Guid CategoryId,
        string ExpenseName,
        string Description,
        RecurrenceUnit RecurrenceUnit,
        int RecurrenceInterval,
        DateOnly StartTrackingDate,
        DateOnly? EndDate);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/v1/expenses/{expenseId:Guid}", async (
        Guid expenseId,
        [FromBody]
        UpdateExpenseRequest request,
        ICommandHandler<UpdateExpenseCommand> handler,
        CancellationToken cancellationToken) =>
        {
            Result result = await handler.Handle(
                new UpdateExpenseCommand(
                    expenseId,
                    request.CategoryId,
                    request.ExpenseName,
                    request.Description,
                    request.RecurrenceUnit,
                    request.RecurrenceInterval,
                    request.StartTrackingDate,
                    request.EndDate
                    ),
                cancellationToken).ConfigureAwait(false);
            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.Expenses)
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}
