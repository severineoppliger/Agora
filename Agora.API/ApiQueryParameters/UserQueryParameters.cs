using Agora.Core.Interfaces.QueryParameters;

namespace Agora.API.ApiQueryParameters;

/// <summary>
/// Query parameters for filtering and sorting a list of <c>User</c>.
/// </summary>
public class UserQueryParameters : IUserQueryParameters
{
    /// <summary>
    /// Filter users by username using substring search.
    /// </summary>
    public string? Username { get; set; }
    
    /// <summary>
    /// Filter users by email address using substring search.
    /// </summary>
    public string? Email { get; set; }
    
    /// <summary>
    /// Filter users having at least this amount in <c>Credit</c>.
    /// </summary>
    public int? MinCredit { get; set; }
    
    /// <summary>
    /// Filter users having at most this amount in <c>Credit</c>.    /// </summary>
    public int? MaxCredit { get; set; }
    
    /// <summary>
    /// Filter users created after or on this date.
    /// </summary>
    public DateTime? MinCreatedAt { get; set; }
    
    /// <summary>
    /// Filter users created before or on this date.
    /// </summary>
    public DateTime? MaxCreatedAt { get; set; }
    
    /// <summary>
    /// Property name to sort the results by.
    /// Allowed values: "id" (default), "username", "email", "credit", "createdat" or "lastloginat".
    /// </summary>
    public string? SortBy { get; set; }
    
    /// <summary>
    /// Indicates whether the sorting should be in descending order. Default is false (ascending).
    /// </summary>
    public bool SortDesc { get; set; }
}