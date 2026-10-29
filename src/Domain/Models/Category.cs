using SharedKernel;

namespace Domain.Models;

/// <summary>
/// PFCv2 Designations, as Referenced from Stripe API Documentation
/// </summary>
public class Category
{
    public Guid CategoryId { get; set; }
    public bool IsUserDefined { get; set; } = false;
    /// <summary>
    /// Mapped from the PFCv2 Primary designation
    /// </summary>
    public string CategoryGroupName { get; set; } = string.Empty;
    /// <summary>
    /// Mapped from the PFCv2 Detailed designation
    /// </summary>
    public string CategoryName { get; set; } = string.Empty;
    /// <summary>
    /// Mapped from the PFCv2 Description designation
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    public static Category Create(
        string categoryGroupName,
        string categoryName,
        string description
    )
        {
            return new Category
            {
                IsUserDefined = true,
                CategoryGroupName = categoryGroupName,
                CategoryName = categoryName,
                Description = description
            };
        }

    public void Update(
        string categoryGroupName,
        string categoryName,
        string description)
    {
        if(IsUserDefined)
        {
            CategoryGroupName = categoryGroupName;
            CategoryName = categoryName;
            Description = description;
        }
    }
}
public static class CategoryErrors
{
    public static Error NotFound(Guid categoryId) => Error.NotFound(
        "Categories.NotFound",
        $"The category with the Id = '{categoryId}' was not found");
    public static Error NotUserDefined(Guid categoryId) => Error.Problem(
            "Categories.Problem", 
            $"Unable to edit since Category: {categoryId} is not user defined.");
}
