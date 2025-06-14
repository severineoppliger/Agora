using Agora.Core.BusinessRules.Interfaces;
using Agora.Core.Common;
using Agora.Core.Enums;
using Agora.Core.Models;

namespace Agora.Core.BusinessRules;

/// <summary>
/// Determine if a certain user can see some entity.
/// </summary>
public class AuthorizationBusinessRules : IAuthorizationBusinessRules
{
    // Admin sees posts with any status
    // Non-admin sees:
    //   - All their posts without the deleted ones if they target to see their posts
    //   - Only active posts of others
    public bool CanViewPost(Post post, UserContext userContext)
    {
        bool isOwner = post.OwnerUserId == userContext.UserId;
        return userContext.IsAdmin 
               || isOwner && post.Status != PostStatus.Deleted
               || !isOwner && post.Status is PostStatus.Active or PostStatus.InTransactionActive;
    }
    
    public bool CanViewTransaction(Transaction transaction, UserContext userContext)
    {
        return IsInvolvedOrAdmin(transaction, userContext);
    }
    
    public bool CanModifyTransaction(Transaction transaction, UserContext userContext)
    {
        return IsInvolvedOrAdmin(transaction, userContext);
    }

    private bool IsInvolvedOrAdmin(Transaction transaction, UserContext userContext)
    {
        bool isInvolved = transaction.BuyerId == userContext.UserId || transaction.SellerId == userContext.UserId;
        return userContext.IsAdmin || isInvolved;
    }
}