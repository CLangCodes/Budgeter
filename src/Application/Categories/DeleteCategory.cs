using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Categories;

public sealed record DeleteCategoryCommand(Guid CategoryId) : ICommand;
internal sealed class DeleteCategoryCommandHandler(IApplicationDbContext context)
    : ICommandHandler<DeleteCategoryCommand>
{
    public async Task<Result> Handle(DeleteCategoryCommand command, CancellationToken cancellationToken)
    {
        Category? category = await context.Categories.SingleOrDefaultAsync(
            g => g.CategoryId == command.CategoryId, cancellationToken);

        if (category is null)
            {return Result.Failure(CategoryErrors.NotFound(command.CategoryId));}
        if (!category.IsUserDefined)
            {return Result.Failure(CategoryErrors.NotUserDefined(command.CategoryId));}
        context.Categories.Remove(category);
        
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}