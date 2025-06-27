namespace Agora.Core.Interfaces.QueryParameters;

/// <summary>
/// Interface that defines query parameters for filtering and sorting a list of <c>User</c>.
/// Implemented by classes used to handle incoming query parameters in API endpoints.
/// </summary>
public interface IUserQueryParameters
{
    /// <summary>
    /// Filters users by username using substring search.
    /// </summary>
    public string? Username { get; set; }
    
    /// <summary>
    /// Filters users by email address using substring search.
    /// </summary>
    public string? Email { get; set; }
    
    /// <summary>
    /// Filters users having at least this amount in <c>Credit</c>.
    /// </summary>
    public int? MinCredit { get; set; }
    
    /// <summary>
    /// Filters users having at most this amount in <c>Credit</c>.
    /// /// </summary>
    public int? MaxCredit { get; set; }
    
    /// <summary>
    /// Filters users created after or on this date.
    /// </summary>
    public DateTime? MinCreatedAt { get; set; }
    
    /// <summary>
    /// Filters users created before or on this date.
    /// </summary>
    public DateTime? MaxCreatedAt { get; set; }
    
    /// <summary>
    /// Property name to sort the results by.
    /// Allowed values are given in class <c>SortByOptions.User</c>.
    /// </summary>
    public string? SortBy { get; set; }
    
    /// <summary>
    /// Indicates whether the sorting should be in descending order. Default is false (ascending).
    /// </summary>
    public bool SortDesc { get; set; }
}