using Agora.Core.Interfaces.QueryParameters;

namespace Agora.API.ApiQueryParameters;

/// <summary>
/// Query parameters for filtering and sorting a list of <c>PostCategory</c> in API endpoints.
/// </summary>
public class PostCategoryQueryParameters : IPostCategoryQueryParameters
{
    /// <inheritdoc />
    public string? Name { get; set; }
    
    /// <inheritdoc />
    public string? SortBy { get; set; }
    
    /// <inheritdoc />
    public bool SortDesc { get; set; }
}