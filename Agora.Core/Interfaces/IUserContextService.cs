using Agora.Core.Common;

namespace Agora.Core.Interfaces;

public interface IUserContextService
{
    UserContext GetCurrentUserContext();
    bool IsAuthenticated();
}