using Agora.Core.Models;

namespace Agora.Core.Interfaces;

/// <summary>
/// Provides methods to access information about the current <c>User</c> context.
/// </summary>
public interface IUserContextService
{
    /// <summary>
    /// Gets the <see cref="UserContext"/> of the current user.
    /// </summary>
    /// <returns>
    /// A <see cref="UserContext"/> instance containing user identity and role information.
    /// </returns>
    UserContext GetCurrentUserContext();
    
    /// <summary>
    /// Determines whether the current user is authenticated.
    /// </summary>
    /// <returns>
    /// <c>true</c> if the user is authenticated; otherwise, <c>false</c>.
    /// </returns>
    bool IsAuthenticated();
}