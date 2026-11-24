using Application.Abstractions.Authorization;
using Application.Abstractions.Messaging;
using Application.Categories;
using BudgetBuddy.Extensions;
using BudgetBuddy.Http;
using SharedKernel;

namespace BudgetBuddy.Endpoints.Categories;

internal sealed class GetCategoryByIdEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/v1/categories/{categoryId:Guid}", async (
        Guid categoryId,
        IQueryHandler<GetCategoryByIdQuery, GetCategoryResult> handler,
        CancellationToken cancellationToken) =>
        {
            Result<GetCategoryResult> result = await handler.Handle(
                new GetCategoryByIdQuery(categoryId), cancellationToken).ConfigureAwait(false);
            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .HasPermission(Permissions.CategoriesRead)
        .WithTags(Tags.Categories)
        .Produces<GetCategoryResult>()
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}
