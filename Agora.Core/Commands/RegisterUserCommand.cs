namespace Agora.Core.Commands;

/// <summary>
/// Command to register a new <c>User</c> with the required credentials.
/// </summary>
public class RegisterUserCommand
{
    /// <summary>
    /// Desired username for the new user.
    /// </summary>
    public string UserName { get; init; } = String.Empty;
    
    /// <summary>
    /// Email address of the new user.
    /// </summary>
    public string Email { get; init; } = String.Empty;
    
    /// <summary>
    /// Password for the new user account.
    /// </summary>
    public string Password { get; init; } = String.Empty;
}