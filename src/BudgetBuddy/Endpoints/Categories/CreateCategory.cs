using Application.Abstractions.Authorization;
using Application.Abstractions.Messaging;
using Application.Categories;
using BudgetBuddy.Extensions;
using BudgetBuddy.Http;
using SharedKernel;

namespace BudgetBuddy.Endpoints.Categories;

internal sealed class CreateCategoryEndpoint : IEndpoint
{
    public sealed record Request(
        string CategoryGroupName,
        string CategoryName,
        string Description);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/v1/categories", async (
        Request request,
        ICommandHandler<CreateCategoryCommand, Guid> handler,
        CancellationToken cancellationToken) =>
        {
            var command = new CreateCategoryCommand(
                request.CategoryGroupName,
                request.CategoryName,
                request.Description);

            Result<Guid> result = await handler.Handle(
                command,
                cancellationToken).ConfigureAwait(false);
            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .HasPermission(Permissions.CategoriesWrite)
        .WithTags(Tags.Categories)
        .Produces<Guid>()
        .ProducesProblem(StatusCodes.Status400BadRequest);
    }
}
