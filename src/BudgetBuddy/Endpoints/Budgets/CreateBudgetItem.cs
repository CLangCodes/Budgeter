using Application.Abstractions.Authorization;
using Application.Abstractions.Messaging;
using Application.Budgets;
using BudgetBuddy.Extensions;
using BudgetBuddy.Http;
using Domain.Models;
using SharedKernel;

namespace BudgetBuddy.Endpoints.Budgets;

internal sealed class CreateBudgetItemEndpoint : IEndpoint
{
    public sealed record Request(
        Guid CategoryId,
        Guid? AccountId,
        string BudgetItemName,
        string Description,
        RecurrenceUnit RecurrenceUnit,
        int RecurrenceInterval,
        DateOnly StartTrackingDate,
        DateOnly? EndDate,
        decimal Amount);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/v1/budgets", async (
        Request request,
        ICommandHandler<CreateBudgetItemCommand, Guid> handler,
        CancellationToken cancellationToken) =>
        {
            var command = new CreateBudgetItemCommand(
                request.CategoryId,
                request.AccountId,
                request.BudgetItemName,
                request.Description,
                request.RecurrenceUnit,
                request.RecurrenceInterval,
                request.StartTrackingDate,
                request.EndDate,
                request.Amount);

            Result<Guid> result = await handler.Handle(
                command,
                cancellationToken).ConfigureAwait(false);
            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .HasPermission(Permissions.BudgetsWrite)
        .WithTags(Tags.Budgets)
        .Produces<Guid>()
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}
