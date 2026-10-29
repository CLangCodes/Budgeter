using Application.Categories;
using Application.Abstractions.Messaging;

namespace BudgetBuddy.Endpoints.Categories;

internal sealed class GetCategoryByIdEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/v1/categories/{categoryId:Guid}", async (HttpContext httpContext,
        Guid categoryId,
        IQueryHandler<GetCategoryByIdQuery, GetCategoryResult> handler,
        CancellationToken cancellationToken) =>
        {
            try
            {
                var result = await handler.Handle(
                    new GetCategoryByIdQuery(categoryId), cancellationToken).ConfigureAwait(false);
                return Results.Ok(result.Value);                
            } catch
            {
                return Results.Problem($"There was a problem retrieving the Category with id: {categoryId}.");
            }
        })
        .WithTags(Tags.Categories)
        .Accepts<GetCategoryByIdQuery>("Categories")
        .Produces<GetCategoryResult>()
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}