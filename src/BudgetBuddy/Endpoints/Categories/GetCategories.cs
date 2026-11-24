using Application.Abstractions.Authorization;
using Application.Abstractions.Messaging;
using Application.Categories;
using BudgetBuddy.Extensions;
using BudgetBuddy.Http;
using SharedKernel;

namespace BudgetBuddy.Endpoints.Categories;

internal sealed class GetCategoriesEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/v1/categories/", async (
        IQueryHandler<GetCategoriesQuery, GetCategoriesResult> handler,
        CancellationToken cancellationToken) =>
        {
            Result<GetCategoriesResult> result = await handler.Handle(
                new GetCategoriesQuery(), cancellationToken).ConfigureAwait(false);
            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .HasPermission(Permissions.CategoriesRead)
        .WithTags(Tags.Categories)
        .Produces<GetCategoriesResult>();
    }
}
