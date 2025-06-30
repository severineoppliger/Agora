using Agora.Core.Commands;
using Agora.Core.Models.Entities;
using Agora.Core.Shared;

namespace Agora.Core.Interfaces;

/// <summary>
/// Defines authentication-related operations such as user registration, login, and logout.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Registers a new user with the specified registration information.
    /// </summary>
    /// <param name="command">The user's registration data including email, username, and password.</param>
    /// <returns>A <see cref="Result{T}"/> containing the created <see cref="User"/> on success, or an error on failure.</returns>
    Task<Result<User>> RegisterAsync(RegisterUserCommand command);
    
    /// <summary>
    /// Attempts to log in a user with the provided credentials.
    /// </summary>
    /// <param name="command">The user's credentials (email address and password).</param>
    /// <returns>A <see cref="Result"/> indicating whether the login was successful or not.</returns>
    Task<Result> LoginAsync(SignInUserCommand command);
    
    /// <summary>
    /// Logs out the currently authenticated user.
    /// </summary>
    Task LogoutAsync();
}