using Agora.Core.Common;
using Agora.Core.Interfaces.Filters;
using Agora.Core.Models;

namespace Agora.Core.Interfaces.BusinessServices;

/// <summary>
/// Provides business logic related to user management,
/// including access control and profile retrieval.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Retrieves all <c>User</c>.
    /// </summary>
    /// <param name="userQueryParameters">Filter criteria to apply when querying users.</param>
    /// <returns>A successful Result wrapping a list of users, or failure if an error occurs.</returns>
    Task<Result<IReadOnlyList<User>>> GetAllUsersAsync(IUserFilter userQueryParameters);
    
    /// <summary>
    /// Retrieves a single <c>User</c> by its ID.
    /// </summary>
    /// <param name="userId">The ID of the user to retrieve.</param>
    /// <param name="userContext">Context of the current user requesting user details.</param>
    /// <returns>
    /// Success wrapping the <c>User</c> if found and authorized,
    /// failure with NotFound if missing.
    /// </returns>
    Task<Result<User>> GetUserByIdAsync(string userId, UserContext userContext);
    
    //TODO
    Task<Result> TransferCreditAsync(User buyer, User seller, int price);
}