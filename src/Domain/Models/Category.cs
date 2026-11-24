using SharedKernel;

namespace Domain.Models;

/// <summary>
/// PFCv2 Designations, as Referenced from Stripe API Documentation
/// </summary>
public class Category : Entity
{
    public Guid CategoryId { get; private set; }
    public bool IsUserDefined { get; private set; }
    /// <summary>
    /// Mapped from the PFCv2 Primary designation
    /// </summary>
    public string CategoryGroupName { get; private set; } = string.Empty;
    /// <summary>
    /// Mapped from the PFCv2 Detailed designation
    /// </summary>
    public string CategoryName { get; private set; } = string.Empty;
    /// <summary>
    /// Mapped from the PFCv2 Description designation
    /// </summary>
    public string Description { get; private set; } = string.Empty;

    public static Category Create(
        string categoryGroupName,
        string categoryName,
        string description)
    {
        return new Category
        {
            IsUserDefined = true,
            CategoryGroupName = categoryGroupName,
            CategoryName = categoryName,
            Description = description
        };
    }

    /// <summary>
    /// Only user-defined categories may be edited; system PFCv2 categories are fixed.
    /// </summary>
    public Result Update(
        string categoryGroupName,
        string categoryName,
        string description)
    {
        if (!IsUserDefined)
        {
            return Result.Failure(CategoryErrors.NotUserDefined(CategoryId));
        }

        CategoryGroupName = categoryGroupName;
        CategoryName = categoryName;
        Description = description;
        return Result.Success();
    }

    /// <summary>
    /// Only user-defined categories may be deleted; system PFCv2 categories are fixed.
    /// </summary>
    public Result CanDelete() =>
        IsUserDefined
            ? Result.Success()
            : Result.Failure(CategoryErrors.NotUserDefined(CategoryId));
}

public static class CategoryErrors
{
    public static Error NotFound(Guid categoryId) => Error.NotFound(
        "Categories.NotFound",
        $"The category with the Id = '{categoryId}' was not found");
    public static Error NotUserDefined(Guid categoryId) => Error.Problem(
            "Categories.Problem",
            $"Unable to modify since Category: {categoryId} is not user defined.");
}
