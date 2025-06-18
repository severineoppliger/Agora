namespace Agora.API.DTOs.User;

/// <summary>
/// Data Transfer Object used for controller output.
/// It represents a summary view of a <c>User</c>, typically used in lists or search results.
/// </summary>
public class UserSummaryDto
{
    /// <summary>
    /// Unique identifier of the user.
    /// </summary>
    public string Id { get; set; } = String.Empty;
    
    /// <summary>
    /// Username of the user.
    /// </summary>
    public string UserName { get; set; } = String.Empty;
    
    /// <summary>
    /// Email address of the user.
    /// </summary>
    public string Email { get; set; } = String.Empty;
    
    /// <summary>
    /// Current credit balance of the user (in Kairos credits).
    /// </summary>
    public int Credit { get; set; }
}