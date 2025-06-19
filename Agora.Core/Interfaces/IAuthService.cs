using Agora.Core.Common;
using Agora.Core.Models;
using Agora.Core.Models.Requests;

namespace Agora.Core.Interfaces;

/// <summary>
/// Defines authentication-related operations such as user registration, login, and logout.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Registers a new user with the specified registration information.
    /// </summary>
    /// <param name="registrationInfo">The user's registration data including email, username, and password.</param>
    /// <returns>A <see cref="Result{T}"/> containing the created <see cref="User"/> on success, or an error on failure.</returns>
    Task<Result<User>> RegisterAsync(UserRegistrationInfo registrationInfo);
    
    /// <summary>
    /// Attempts to log in a user with the provided credentials.
    /// </summary>
    /// <param name="signInInfo">The user's credentials (email address and password).</param>
    /// <returns>A <see cref="Result"/> indicating whether the login was successful or not.</returns>
    Task<Result> LoginAsync(UserSignInInfo signInInfo);
    
    /// <summary>
    /// Logs out the currently authenticated user.
    /// </summary>
    Task LogoutAsync();
}