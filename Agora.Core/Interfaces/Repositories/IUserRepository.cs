using Agora.Core.Interfaces.QueryParameters;
using Agora.Core.Models;
using Agora.Core.Models.Entities;
using Microsoft.AspNetCore.Identity;

namespace Agora.Core.Interfaces.Repositories;

public interface IUserRepository
{
    /// <summary>
    /// Retrieves all users, optionally filtered by the provided queryParameters.
    /// </summary>
    /// <param name="queryParameters">An optional queryParameters to apply to the user query.</param>
    /// <returns>A list of <see cref="User"/> objects matching the queryParameters.</returns>
    Task<IReadOnlyList<User>> GetAllUsersAsync(IUserQueryParameters queryParameters);
    
    /// <summary>
    /// Retrieves a user by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the user.</param>
    /// <returns>
    /// The <see cref="User"/> if found; otherwise, <c>null</c>.
    /// </returns>
    Task<User?> GetUserByIdAsync(string id);
    
    /// <summary>
    /// Retrieves a user by its email.
    /// </summary>
    /// <param name="email">The email of the user.</param>
    /// <returns>
    /// The <see cref="User"/> if found; otherwise, <c>null</c>.
    /// </returns>
    Task<User?> GetUserByEmailAsync(string email);
    
    /// <summary>
    /// Retrieves a user by its username.
    /// </summary>
    /// <param name="username">The username of the user.</param>
    /// <returns>
    /// The <see cref="User"/> if found; otherwise, <c>null</c>.
    /// </returns>
    public Task<User?> GetUserByUsernameAsync(string username);
    
    /// <summary>
    /// Adds a new user to the repository, after hashing the password.
    /// </summary>
    /// <param name="user">The post to add.</param>
    /// <param name="password">The new user password.</param>
    Task<IdentityResult> AddUserAsync(User user, string password);
    
    /// <summary>
    /// Persists all pending changes about a user in the repository to the database.
    /// </summary>
    /// <returns>
    /// <c>true</c> if the changes were saved successfully; otherwise, <c>false</c>.
    /// </returns>
    Task<IdentityResult> UpdateUserAsync(User user);
    
    /// <summary>
    /// Checks whether a user with the specified ID exists.
    /// </summary>
    /// <param name="id">The unique identifier of the user.</param>
    /// <returns>
    /// <c>true</c> if the user exists; otherwise, <c>false</c>.
    /// </returns>
    Task<bool> UserExistsAsync(string id);
}