using Agora.Core.Interfaces.QueryParameters;
using Agora.Core.Models;
using Agora.Core.Models.Entities;
using Agora.Core.Shared;

namespace Agora.Core.Interfaces.DomainServices;

/// <summary>
/// Provides business logic related to user management,
/// including access control and profile retrieval.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Retrieves all <c>User</c>.
    /// </summary>
    /// <param name="queryParams">Filter criteria to apply when querying users.</param>
    /// <returns>
    /// A successful <see cref="Result{T}"/> wrapping a list of <c>User</c>,
    /// or failure if an error occurs.
    /// </returns>
    Task<Result<IReadOnlyList<User>>> GetAllUsersAsync(IUserQueryParameters queryParams);
    
    /// <summary>
    /// Retrieves a single <c>User</c> by its ID.
    /// </summary>
    /// <param name="userId">The ID of the user to retrieve.</param>
    /// <param name="userContext">Context of the current user requesting user details.</param>
    /// <returns>
    /// A successful <see cref="Result{T}"/> wrapping the <c>User</c> if found and authorized,
    /// failure with NotFound if missing.
    /// </returns>
    Task<Result<User>> GetUserByIdAsync(string userId, UserContext userContext);
    
    /// <summary>
    /// Retrieves the email address of a user based on their username.
    /// </summary>
    /// <param name="userName">The username of the user whose email address is requested.</param>
    /// <returns>
    /// A successful <see cref="Result{T}"/> wrapping the <c>User</c> if found,
    /// failure with NotFound if missing.
    /// </returns>
    Task<Result<User>> GetUserEmailByUsernameAsync(string userName);
    
    /// <summary>
    /// Transfers a specified amount of Kairos from the buyer to the seller.
    /// </summary>
    /// <param name="buyer">The <c>User</c> who will have their credit reduced.</param>
    /// <param name="seller">The <c>User</c> who will receive the credit.</param>
    /// <param name="price">The amount of Kairos to transfer.</param>
    /// <returns>
    /// A <see cref="Result"/> indicating success or failure of the credit transfer operation.
    /// </returns>
    Task<Result> TransferCreditAsync(User buyer, User seller, int price);
}