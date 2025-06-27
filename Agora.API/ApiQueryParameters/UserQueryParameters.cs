using Agora.Core.Interfaces.QueryParameters;

namespace Agora.API.ApiQueryParameters;

/// <summary>
/// Query parameters for filtering and sorting a list of <c>User</c> in API endpoints.
/// </summary>
public class UserQueryParameters : IUserQueryParameters
{
    /// <inheritdoc />
    public string? Username { get; set; }
    
    /// <inheritdoc />
    public string? Email { get; set; }
    
    /// <inheritdoc />
    public int? MinCredit { get; set; }
    
    /// <inheritdoc />
    public int? MaxCredit { get; set; }
    
    /// <inheritdoc />
    public DateTime? MinCreatedAt { get; set; }
    
    /// <inheritdoc />
    public DateTime? MaxCreatedAt { get; set; }
    
    /// <inheritdoc />
    public string? SortBy { get; set; }
    
    /// <inheritdoc />
    public bool SortDesc { get; set; }
}