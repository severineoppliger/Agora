using Agora.Core.Models;
using Agora.Core.Models.Entities;

namespace Agora.Core.BusinessRules.Interfaces;

/// <summary>
/// Determine if a certain user can see, create, modify or delete some entity.
/// </summary>
public interface IAuthorizationValidator
{
    public bool CanPayPrice(User buyer, int price);
    public bool CanViewUser(string id, UserContext userContext);
    public bool CanViewPost(Post post, UserContext userContext);
    public bool CanManagePost(Post post, UserContext userContext);
    public bool CanViewTransaction(Transaction transaction, UserContext userContext);
    public bool CanCreateTransaction(Transaction transaction, UserContext userContext);
    public bool CanManageTransaction(Transaction transaction, UserContext userContext);
}