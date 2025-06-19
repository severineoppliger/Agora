using Agora.Core.Interfaces.QueryParameters;

namespace Agora.API.ApiQueryParameters;

/// <summary>
/// Query parameters for filtering and sorting a list of <c>Post</c>
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
    /// Filter posts by the name of the post category using substring search.
    /// </summary>
    public string? PostCategoryName { get; set; }
    
    /// <summary>
    /// Filter posts created by a specific user using substring search on username.
    /// </summary>
    public string? UserName { get; set; }
    
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