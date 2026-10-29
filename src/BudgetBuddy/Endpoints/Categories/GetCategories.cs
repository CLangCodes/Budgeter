using Application.Categories;
using Application.Abstractions.Messaging;

namespace BudgetBuddy.Endpoints.Categories;

internal sealed class GetCategoriesEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/v1/categories/", async (HttpContext httpContext,
        IQueryHandler<GetCategoriesQuery, GetCategoriesResult> handler,
        CancellationToken cancellationToken) =>
        {
            try
            {
                var result = await handler.Handle(
                    new GetCategoriesQuery(), cancellationToken).ConfigureAwait(false);
                return Results.Ok(result.Value);
            } catch
            {
                return Results.Problem("There was a problem retrieving Categories.");
            }
        })
        .Accepts<GetCategoriesQuery>("Categories")
        .WithTags(Tags.Categories)
        .Produces<GetCategoriesResult>();
    }
}