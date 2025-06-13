using Agora.Core.BusinessRules.Interfaces;
using Agora.Core.Enums;

namespace Agora.Core.BusinessRules;

public class VisibilityBusinessRules : IVisibilityBusinessRules
{
    // Admin sees posts with any status
    // Non-admin sees:
    //   - All their posts without the deleted ones if they target to see their posts
    //   - Only active posts of others
    public bool CanViewPost(PostStatus postStatus, bool isAdmin, bool isOwner)
    {
        return isAdmin 
               || isOwner && postStatus != PostStatus.Deleted
               || !isOwner && postStatus is PostStatus.Active or PostStatus.InTransactionActive;
    }
}