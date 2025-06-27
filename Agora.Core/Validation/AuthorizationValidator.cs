using Agora.Core.Enums;
using Agora.Core.Models;
using Agora.Core.Models.Entities;
using Agora.Core.Validation.Interfaces;

namespace Agora.Core.Validation;

/// <summary>
/// Default implementation of <see cref="IAuthorizationValidator"/>.
/// </summary>
public class AuthorizationValidator : IAuthorizationValidator
{
    /// <inheritdoc />
    public bool CanPayPrice(User buyer, int price)
    {
        return buyer.Credit >= price;
    }

    /// <inheritdoc />
    public bool CanViewUser(string userId, UserContext userContext)
    {
        return userContext.IsAdmin || userContext.UserId == userId;
    }

    /// <inheritdoc />
    public bool CanViewPost(Post post, UserContext userContext)
    {
        bool isOwner = post.OwnerUserId == userContext.UserId;
        return (userContext.IsAdmin) ||
               (isOwner && post.Status is PostStatus.Active or PostStatus.Inactive)
               || (!isOwner && post.Status == PostStatus.Active);
    }

    /// <inheritdoc />
    public bool CanManagePost(Post post, UserContext userContext)
    {
        return userContext.IsAdmin || userContext.UserId == post.OwnerUserId;
    }
    
    /// <inheritdoc />
    public bool CanViewTransaction(Transaction transaction, UserContext userContext)
    {
        return IsInvolvedOrAdmin(transaction, userContext);
    }
    
    /// <inheritdoc />
    public bool CanCreateTransaction(Transaction transaction, UserContext userContext)
    { 
        return IsInvolved(transaction, userContext);
    }
    
    /// <inheritdoc />
    public bool CanManageTransaction(Transaction transaction, UserContext userContext)
    {
        return IsInvolvedOrAdmin(transaction, userContext);
    }
    
    /// <summary>
    /// Determines whether the current user is involved in the specified transaction,
    /// either as the buyer or the seller.
    /// </summary>
    /// <param name="transaction">The transaction to check involvement in.</param>
    /// <param name="userContext">The current user's context.</param>
    /// <returns>
    /// <c>true</c> if the user is either the buyer or the seller of the transaction; otherwise, <c>false</c>.
    /// </returns>
    private bool IsInvolved(Transaction transaction, UserContext userContext)
    {
        return transaction.BuyerId == userContext.UserId || transaction.SellerId == userContext.UserId;
    }
    
    /// <summary>
    /// Determines whether the current user is involved in the specified transaction
    /// (either as the buyer or the seller), or has administrative privileges.
    /// </summary>
    /// <param name="transaction">The transaction to check involvement or admin access for.</param>
    /// <param name="userContext">The current user's context.</param>
    /// <returns>
    /// <c>true</c> if the user is either the buyer or the seller of the transaction,
    /// or if the user has administrative rights; otherwise, <c>false</c>.
    /// </returns>
    private bool IsInvolvedOrAdmin(Transaction transaction, UserContext userContext)
    {
        bool isInvolved = IsInvolved(transaction, userContext);
        return userContext.IsAdmin || isInvolved;
    }
}