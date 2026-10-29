using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Models;
using SharedKernel;

namespace Application.Categories;

public sealed record CreateCategoryCommand(
    string CategoryGroupName,
    string CategoryName,
    string Description) : ICommand<Guid>;

internal sealed class CreateCategoryCommandHandler(
    IApplicationDbContext context
) : ICommandHandler<CreateCategoryCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateCategoryCommand command, CancellationToken cancellationToken)
    {
        Category category = Category.Create(
            command.CategoryGroupName,
            command.CategoryName,
            command.Description ?? string.Empty
        );

        context.Categories.Add(category);
        await context.SaveChangesAsync(cancellationToken);
        return category.CategoryId;
    }
}