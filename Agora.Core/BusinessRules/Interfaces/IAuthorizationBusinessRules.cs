using Agora.Core.Common;
using Agora.Core.Models;

namespace Agora.Core.BusinessRules.Interfaces;

public interface IAuthorizationBusinessRules
{
    public bool CanViewPost(Post post, UserContext userContext);
    public bool CanViewTransaction(Transaction transaction, UserContext userContext);
    public bool CanModifyTransaction(Transaction transaction, UserContext userContext);
}