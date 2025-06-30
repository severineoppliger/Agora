using Agora.Core.Models;
using Agora.Core.Models.Entities;

namespace Agora.Core.Validation.Interfaces;

/// <summary>
/// Interface defining methods to check if a user is authorized
/// to view, create, modify, or delete various entities.
/// </summary>
public interface IAuthorizationValidator
{
    /// <summary>
    /// Determines whether a buyer (<c>User</c>) in a transaction is allowed to pay a specified price,
    /// based on their <c>Credit</c>.
    /// </summary>
    /// <param name="buyer">The user attempting to pay.</param>
    /// <param name="price">The price to be paid.</param>
    /// <returns>True if payment is allowed; otherwise, false.</returns>
    public bool CanPayPrice(User buyer, int price);
    
    /// <summary>
    /// Determines whether a <c>User</c> is allowed to view another user's information, based on current user's context.
    /// </summary>
    /// <param name="id">The ID of the user to view.</param>
    /// <param name="userContext">The current user's context.</param>
    /// <returns>True if viewing is permitted; otherwise, false.</returns>
    public bool CanViewUser(string id, UserContext userContext);
    
    /// <summary>
    /// Determines whether a <c>User</c> is allowed to view a specific <c>Post</c>, based on their user's context.
    /// </summary>
    /// <param name="post">The post to view.</param>
    /// <param name="userContext">The current user's context.</param>
    /// <returns>True if viewing is permitted; otherwise, false.</returns>
    public bool CanViewPost(Post post, UserContext userContext);
    
    /// <summary>
    /// Determines whether a <c>User</c> is allowed to manage (edit/delete) a specific <c>Post</c>, based on their user's context.
    /// </summary>
    public bool CanManagePost(Post post, UserContext userContext);
    
    /// <summary>
    /// Determines whether a <c>User</c> is allowed to view a <c>Transaction</c>, based on their user's context.
    /// </summary>
    /// <param name="transaction">The transaction to view.</param>
    /// <param name="userContext">The current user's context.</param>
    /// <returns>True if viewing is permitted; otherwise, false.</returns>
    public bool CanViewTransaction(Transaction transaction, UserContext userContext);
    
    /// <summary>
    /// Determines whether a <c>User</c> is allowed to create a <c>Transaction</c>, based on their user's context.
    /// </summary>
    /// <param name="transaction">The transaction to create.</param>
    /// <param name="userContext">The current user's context.</param>
    /// <returns>True if creating is permitted; otherwise, false.</returns>
    public bool CanCreateTransaction(Transaction transaction, UserContext userContext);
    
    /// <summary>
    /// Determines whether a <c>User</c> context is allowed to manage (edit/delete) a <c>Transaction</c>,
    /// based on their user's context.
    /// </summary>
    /// <param name="transaction">The transaction to manage.</param>
    /// <param name="userContext">The current user's context.</param>
    /// <returns>True if managing is permitted; otherwise, false.</returns>
    public bool CanManageTransaction(Transaction transaction, UserContext userContext);
}