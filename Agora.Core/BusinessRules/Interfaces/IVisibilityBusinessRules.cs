using Agora.Core.Enums;

namespace Agora.Core.BusinessRules.Interfaces;

public interface IVisibilityBusinessRules
{
    public bool CanViewPost(PostStatus postStatus, bool isAdmin, bool isOwner);
}