namespace Agora.Core.Commands;

/// <summary>
/// Command to authenticate a <c>User</c> with email and password.
/// </summary>
public class SignInUserCommand
{
    /// <summary>
    /// Email address of the user attempting to sign in.
    /// </summary>
    public string Email { get; init; } = String.Empty;
    
    /// <summary>
    /// Password of the user attempting to sign in.
    /// </summary>
    public string Password { get; init; } = String.Empty;
}