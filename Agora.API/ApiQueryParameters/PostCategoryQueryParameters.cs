using Agora.Core.Interfaces.QueryParameters;

namespace Agora.API.ApiQueryParameters;

/// <summary>
/// Query parameters for filtering and sorting a list of <c>PostCategory</c>
/// </summary>
public class PostCategoryQueryParameters : IPostCategoryQueryParameters
{
    /// <summary>
    /// Filter by name of the post category using substring search.
    /// </summary>
    public string? Name { get; set; }
    
    /// <summary>
    /// Property name to sort the results by. Allowed values: "id" (default), "name".
    /// </summary>
    public string? SortBy { get; set; }
    
    /// <summary>
    /// Indicates whether the sorting should be in descending order. Default is false (ascending).
    /// </summary>
    public bool SortDesc { get; set; }
}