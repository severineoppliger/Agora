using Agora.Core.BusinessRules.Interfaces;
using Agora.Core.Enums;
using Agora.Core.Models;
using Agora.Core.Models.Entities;

namespace Agora.Core.Validation;

/// <summary>
/// Determine if a certain user can see, create, modify or delete some entity.
/// </summary>
public class AuthorizationValidator : IAuthorizationValidator
{
    public bool CanPayPrice(User buyer, int price)
    {
        return buyer.Credit >= price;
    }

    public bool CanViewUser(string userId, UserContext userContext)
    {
        return userContext.IsAdmin || userContext.UserId == userId;
    }

    public bool CanViewPost(Post post, UserContext userContext)
    {
        bool isOwner = post.OwnerUserId == userContext.UserId;
        return (userContext.IsAdmin) ||
               (isOwner && post.Status is PostStatus.Active or PostStatus.Inactive)
               || (!isOwner && post.Status == PostStatus.Active);
    }

    public bool CanManagePost(Post post, UserContext userContext)
    {
        return userContext.IsAdmin || userContext.UserId == post.OwnerUserId;
    }
    
    public bool CanViewTransaction(Transaction transaction, UserContext userContext)
    {
        return IsInvolvedOrAdmin(transaction, userContext);
    }
    
    public bool CanCreateTransaction(Transaction transaction, UserContext userContext)
    { 
        return IsInvolved(transaction, userContext);
    }
    
    public bool CanManageTransaction(Transaction transaction, UserContext userContext)
    {
        return IsInvolvedOrAdmin(transaction, userContext);
    }
    
    private bool IsInvolved(Transaction transaction, UserContext userContext)
    {
        return transaction.BuyerId == userContext.UserId || transaction.SellerId == userContext.UserId;
    }
    
    private bool IsInvolvedOrAdmin(Transaction transaction, UserContext userContext)
    {
        bool isInvolved = transaction.BuyerId == userContext.UserId || transaction.SellerId == userContext.UserId;
        return userContext.IsAdmin || isInvolved;
    }
}