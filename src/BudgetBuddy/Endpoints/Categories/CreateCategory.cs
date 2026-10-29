using Application.Abstractions.Messaging;
using Application.Categories;
using BudgetBuddy.Extensions;
using BudgetBuddy.Infrastructure;
using SharedKernel;

namespace BudgetBuddy.Endpoints.Categories;

internal sealed class CreateCategoryEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/v1/categories", async (
        CreateCategoryCommand command,
        ICommandHandler<CreateCategoryCommand, Guid> handler, 
        CancellationToken cancellationToken) =>
        {
            Result<Guid> result = await handler.Handle(
                command, 
                cancellationToken).ConfigureAwait(false);
            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .Accepts<CreateCategoryCommand>("application/json")
        .WithTags(Tags.Categories)
        .Produces<Guid>()
        .ProducesProblem(StatusCodes.Status400BadRequest);
    }
}
