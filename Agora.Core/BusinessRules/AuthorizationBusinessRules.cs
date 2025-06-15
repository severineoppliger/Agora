using Agora.Core.BusinessRules.Interfaces;
using Agora.Core.Common;
using Agora.Core.Enums;
using Agora.Core.Models;

namespace Agora.Core.BusinessRules;

/// <summary>
/// Determine if a certain user can see, create, modify or delete some entity.
/// </summary>
public class AuthorizationBusinessRules : IAuthorizationBusinessRules
{
    public bool CanManagePost(Post post, UserContext userContext)
    {
        return userContext.IsAdmin || userContext.UserId == post.OwnerUserId;
    }
    
    public bool CanViewTransaction(Transaction transaction, UserContext userContext)
    {
        return IsInvolvedOrAdmin(transaction, userContext);
    }
    
    public bool CanManageTransaction(Transaction transaction, UserContext userContext)
    {
        return IsInvolvedOrAdmin(transaction, userContext);
    }

    private bool IsInvolvedOrAdmin(Transaction transaction, UserContext userContext)
    {
        bool isInvolved = transaction.BuyerId == userContext.UserId || transaction.SellerId == userContext.UserId;
        return userContext.IsAdmin || isInvolved;
    }
}