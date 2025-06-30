using Agora.Core.Interfaces.QueryParameters;

namespace Agora.API.ApiQueryParameters;

/// <summary>
/// Query parameters for filtering and sorting a list of <c>Post</c> in API endpoints.
/// </summary>
public class PostQueryParameters : IPostQueryParameters
{
    /// <inheritdoc />
    public string? TitleOrDescription { get; set; }
    
    /// <inheritdoc />
    public int? MinPrice { get; set; }
    
    /// <inheritdoc />
    public int? MaxPrice { get; set; }
    
    /// <inheritdoc />
    public string? TypeName { get; set; }
    
    /// <inheritdoc />
    public string? PostCategoryName { get; set; }
    
    /// <inheritdoc />
    public string? UserName { get; set; }
    
    /// <inheritdoc />
    public string? SortBy { get; set; }
    
    /// <inheritdoc />
    public bool SortDesc { get; set; } 
}