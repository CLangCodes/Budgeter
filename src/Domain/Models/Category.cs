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
}
public static class CategoryErrors
{
    public static Error NotFound(Guid categoryId) => Error.NotFound(
        "Categories.NotFound",
        $"The category with the Id = '{categoryId}' was not found");
}
