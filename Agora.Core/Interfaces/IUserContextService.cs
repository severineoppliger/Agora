using Agora.Core.Models;

namespace Agora.Core.Interfaces;

public interface IUserContextService
{
    UserContext GetCurrentUserContext();
    bool IsAuthenticated();
}