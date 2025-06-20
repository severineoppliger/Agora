using Agora.Core.Interfaces.QueryParameters;

namespace Agora.Core.Models.DomainQueryParameters;

/// <summary>
/// Query parameters for filtering and sorting a list of <c>Post</c> from the domain layer.
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
    
    /// <summary>
    /// Filter posts by one or more status names. Each status name filter works with substring matching.
    /// </summary>
    public List<string> StatusNames { get; set; } = [];
    
    /// <inheritdoc />
    public string? PostCategoryName { get; set; }
    
    /// <inheritdoc />
    public string? UserName { get; set; }
    
    /// <summary>
    /// Filters posts where the author (owner) ID matches the given user identifier.
    /// </summary>
    public string? UserId { get; set; }
    
    /// <inheritdoc />
    public string? SortBy { get; set; }
    
    /// <inheritdoc />
    public bool SortDesc { get; set; }
}