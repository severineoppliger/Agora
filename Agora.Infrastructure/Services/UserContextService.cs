using System.Security.Authentication;
using System.Security.Claims;
using Agora.Core.Constants;
using Agora.Core.Interfaces;
using Agora.Core.Models;
using Agora.Core.Shared;
using Microsoft.AspNetCore.Http;


namespace Agora.Infrastructure.Services;

/// <inheritdoc />
public class UserContextService(IHttpContextAccessor httpContextAccessor): IUserContextService
{
    /// <inheritdoc />
    public UserContext GetCurrentUserContext()
    {
        HttpContext? httpContext = httpContextAccessor.HttpContext;
    
        if (httpContext?.User?.Identity?.IsAuthenticated != true)
        {
            throw new AuthenticationException(ErrorMessages.User.NotAuthenticated);
        }
        string userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)
                        ?? throw new AuthenticationException(ErrorMessages.User.IdNotFoundInClaims);
        
        bool isAdmin = httpContext.User.IsInRole(Roles.Admin);

        return new UserContext
        {
            UserId = userId,
            IsAdmin = isAdmin
        };
    }

    /// <inheritdoc />
    public bool IsAuthenticated()
    {
        return httpContextAccessor.HttpContext.User?.Identity?.IsAuthenticated == true;
    }
}