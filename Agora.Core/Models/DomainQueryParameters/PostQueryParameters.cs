using Agora.Core.Interfaces.QueryParameters;

namespace Agora.Core.Models.DomainQueryParameters;

/// <summary>
/// Represents query parameters for filtering and sorting posts in the domain layer.
/// </summary>
public class PostQueryParameters : IPostQueryParameters
{
    /// <summary>
    /// Search term for filtering posts by title or description using substring matching.
    /// </summary>
    public string? TitleOrDescription { get; set; }
    
    /// <summary>
    /// Filter posts with <c>Price</c> property at least <c>MinPrice</c>.
    /// </summary>
    public int? MinPrice { get; set; }
    
    /// <summary>
    /// Filter posts with <c>Price</c> property at most <c>MaxPrice</c>.
    /// </summary>
    public int? MaxPrice { get; set; }
    
    /// <summary>
    /// Filter posts by type name. Allowed values: "offer" or "request"
    /// </summary>
    public string? TypeName { get; set; }
    
    /// <summary>
    /// Filter posts by one or more status names. Each status name filter works with substring matching.
    /// </summary>
    public List<string> StatusNames { get; set; } = [];
    
    /// <summary>
    /// Filter posts by the name of the post category using substring search.
    /// </summary>
    public string? PostCategoryName { get; set; }
    
    /// <summary>
    /// Filter posts where the author (owner) username contains the given substring.
    /// </summary>
    public string? UserName { get; set; }
    
    /// <summary>
    /// Filter posts where the author (owner) ID matches the given user identifier.
    /// </summary>
    public string? UserId { get; set; }
    
    /// <summary>
    /// Property name to sort the results by.
    /// Allowed values: "id", "title", "price", "type", "status" (active or inactive),
    ///                 "postcategoryid", "postcategoryname", "username",
    ///                 "createdat" (default in desc) or "updatedat"
    /// </summary>
    public string? SortBy { get; set; }
    
    /// <summary>
    /// Indicates whether the sorting should be in descending order. Default is false (ascending).
    /// </summary>
    public bool SortDesc { get; set; }
}