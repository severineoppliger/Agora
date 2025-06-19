using Agora.Core.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Agora.API.Filters;

public class DisallowAuthenticatedAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (context.HttpContext.User.Identity?.IsAuthenticated == true)
        {
            context.Result =
                new BadRequestObjectResult(ErrorMessages.User.RegisterRequiresNoAuthentication);
        }
    }
}