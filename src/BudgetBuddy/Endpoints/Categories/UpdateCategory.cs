using Application.Abstractions.Messaging;
using Application.Categories;
using BudgetBuddy.Extensions;
using BudgetBuddy.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace BudgetBuddy.Endpoints.Categories;

internal sealed class UpdateCategoryEndpoint : IEndpoint
{
    internal sealed record UpdateCategoryRequest(
        Guid CategoryId,
        string CategoryGroupName,
        string CategoryName,
        string Description);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/v1/categories/{categoryId:Guid}", async (
        Guid categoryId,
        [FromBody]
        UpdateCategoryRequest request,
        ICommandHandler<UpdateCategoryCommand> handler,
        CancellationToken cancellationToken) =>
        {
            Result result = await handler.Handle(
                new UpdateCategoryCommand(
                    categoryId,
                    request.CategoryGroupName,
                    request.CategoryName,
                    request.Description
                    ),
                cancellationToken).ConfigureAwait(false);
            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .Accepts<UpdateCategoryCommand>("Categories")
        .WithTags(Tags.Categories)
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}
