using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Categories;

public sealed record GetCategoriesQuery() : IQuery<GetCategoriesResult>;
public sealed record CategoryDetail(
    Guid CategoryId,
    bool IsUserDefined,
    string CategoryGroupName,
    string GroupName,
    string Description
);
public sealed record GetCategoriesResult(List<CategoryDetail> Categories);


internal class GetCategoriesQueryHandler(IApplicationDbContext context) : IQueryHandler<GetCategoriesQuery, GetCategoriesResult>
{
    public async Task<Result<GetCategoriesResult>> Handle(GetCategoriesQuery query, CancellationToken cancellationToken)
    {
        var categories = await context.Categories
            .AsNoTracking()
            .Select(c => new CategoryDetail(
                c.CategoryId,
                c.IsUserDefined,
                c.CategoryGroupName,
                c.CategoryName,
                c.Description))
            .ToListAsync(cancellationToken);

        return new GetCategoriesResult(categories);
    }
}