using Agora.Core.Common;
using Agora.Core.Models;

namespace Agora.Core.BusinessRules.Interfaces;

/// <summary>
/// Determine if a certain user can see, create, modify or delete some entity.
/// </summary>
public interface IAuthorizationBusinessRules
{
    public bool CanPayPrice(User buyer, int price);
    public bool CanViewUser(string id, UserContext userContext);
    public bool CanViewPost(Post post, UserContext userContext);
    public bool CanManagePost(Post post, UserContext userContext);
    public bool CanViewTransaction(Transaction transaction, UserContext userContext);
    public bool CanManageTransaction(Transaction transaction, UserContext userContext);
}