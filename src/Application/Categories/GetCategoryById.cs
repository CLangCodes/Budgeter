using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Categories;

public sealed record GetCategoryByIdQuery(Guid CategoryId) : IQuery<GetCategoryResult>;
public sealed record GetCategoryResult(
    Guid CategoryId, 
    bool IsUserDefined,
    string CategoryGroupName,
    string CategoryName,
    string Description);

internal sealed class GetCategoryByIdQueryHandler(IApplicationDbContext context)
    : IQueryHandler<GetCategoryByIdQuery, GetCategoryResult>
{
    public async Task<Result<GetCategoryResult>> Handle(GetCategoryByIdQuery query, CancellationToken cancellationToken)
    {
        GetCategoryResult? category = await context.Categories
            .Where(Category => Category.CategoryId == query.CategoryId)
            .Select(Category => new GetCategoryResult(
                Category.CategoryId,
                Category.IsUserDefined,
                Category.CategoryGroupName,
                Category.CategoryName,
                Category.Description
            ))
            .SingleOrDefaultAsync(cancellationToken);
        if (category is null)
        {
            return Result.Failure<GetCategoryResult>(CategoryErrors.NotFound(query.CategoryId));
        }
        return category;
    }
}