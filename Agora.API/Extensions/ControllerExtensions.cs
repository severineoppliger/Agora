using Agora.Core.Common;
using Agora.Core.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Agora.API.Extensions;

public static class ControllerExtensions
{
    public static ActionResult MapErrorResult(this ControllerBase controller, Result result)
    {
        var errors = result.Errors!;
        if (errors.Any(e => e.Type == ErrorType.NotFound))
            return controller.NotFound(errors);
        if (errors.Any(e => e.Type == ErrorType.Unauthorized))
            return controller.Unauthorized(errors);
        if (errors.Any(e => e.Type == ErrorType.Forbidden))
            return controller.Forbid();
        if (errors.Any(e => e.Type == ErrorType.Invalid))
            return controller.BadRequest(errors);
        return controller.StatusCode(500, errors);
    }
}