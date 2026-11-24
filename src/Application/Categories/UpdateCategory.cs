using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Categories;

public sealed record UpdateCategoryCommand(
    Guid CategoryId,
    string CategoryGroupName, 
    string CategoryName, 
    string Description
) : ICommand;


internal sealed class UpdateCategoryCommandHandler(IApplicationDbContext context
    ) : ICommandHandler<UpdateCategoryCommand>
{
    public async Task<Result> Handle(UpdateCategoryCommand command, CancellationToken cancellationToken)
    {
        var category = await context.Categories
            .SingleOrDefaultAsync(g => g.CategoryId == command.CategoryId, cancellationToken);

        if (category is null)
            {return Result.Failure(CategoryErrors.NotFound(command.CategoryId));}
        Result update = category.Update(
            command.CategoryGroupName,
            command.CategoryName,
            command.Description);
        if (update.IsFailure)
            {return update;}

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}