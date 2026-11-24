using Application.Abstractions.Authorization;
using Application.Abstractions.Messaging;
using Application.Categories;
using BudgetBuddy.Extensions;
using BudgetBuddy.Http;
using SharedKernel;

namespace BudgetBuddy.Endpoints.Categories;

internal sealed class DeleteCategoryEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/v1/categories/{categoryId:Guid}", async (
        Guid categoryId,
        ICommandHandler<DeleteCategoryCommand> handler, 
        CancellationToken cancellationToken) =>
        {
            Result result = await handler.Handle(
                new DeleteCategoryCommand(categoryId), 
                cancellationToken).ConfigureAwait(false);
            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .HasPermission(Permissions.CategoriesWrite)
        .WithTags(Tags.Categories)
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}