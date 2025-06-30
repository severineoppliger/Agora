namespace Agora.Core.Models;

/// <summary>
/// Represents the context of the currently authenticated <c>User</c>.
/// </summary>
public class UserContext
{
    /// <summary>
    /// Gets or sets the identifier of the user.
    /// </summary>
    public string UserId { get; init; } = String.Empty;
    
    /// <summary>
    /// Gets or sets a value indicating whether the user has administrative privileges,
    /// i.e. if it has a role equals to <c>Roles.Admin</c>.
    /// </summary>
    public bool IsAdmin { get; init; }
}