namespace Agora.Core.Interfaces.QueryParameters;

/// <summary>
/// Interface that defines query parameters for filtering and sorting a list of <c>PostCategory</c>.
/// Implemented by classes used to handle incoming query parameters in API endpoints.
/// </summary>
public interface IPostCategoryQueryParameters
{
    /// <summary>
    /// Filters by name of the post category using substring search.
    /// </summary>
    public string? Name { get; set; }
    
    /// <summary>
    /// Property name to sort the results by.
    /// Allowed values are given in class <c>SortByOptions.PostCategory</c>.
    /// </summary>
    public string? SortBy { get; set; }
    
    /// <summary>
    /// Indicates whether the sorting should be in descending order. Default is false (ascending).
    /// </summary>
    public bool SortDesc { get; set; }
}