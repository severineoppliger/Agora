using Agora.Core.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Agora.API.Filters;

/// <summary>
/// Authorization filter that prevents authenticated users from accessing the decorated controller or action.
/// Typically used on registration or public-only endpoints, using [DisallowAuthenticated].
/// </summary>
public class DisallowAuthenticatedAttribute : Attribute, IAuthorizationFilter
{
    /// <summary>
    /// Called during authorization to check whether the current user is authenticated.
    /// If the user is authenticated, sets the result to a <see cref="BadRequestObjectResult"/> 
    /// indicating that registration requires no authentication.
    /// </summary>
    /// <param name="context">The authorization filter context.</param>
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (context.HttpContext.User.Identity?.IsAuthenticated == true)
        {
            context.Result =
                new BadRequestObjectResult(ErrorMessages.User.RegisterRequiresNoAuthentication);
        }
    }
}