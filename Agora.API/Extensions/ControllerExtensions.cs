using Agora.Core.Enums;
using Agora.Core.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Agora.API.Extensions;

/// <summary>
/// Provides extension methods for <see cref="ControllerBase"/> to simplify common response patterns.
/// </summary>
public static class ControllerExtensions
{
    /// <summary>
    /// Maps a <see cref="Result"/> containing domain errors to an appropriate <see cref="ActionResult"/> 
    /// based on the type of errors present.
    /// </summary>
    /// <param name="controller">The controller instance on which the extension is called.</param>
    /// <param name="result">The <see cref="Result"/> object containing potential errors to map.</param>
    /// <returns>
    /// An <see cref="ActionResult"/> corresponding to the most relevant error type:
    /// <list type="bullet">
    /// <item><description><c>404 NotFound</c> if any error is of type <c>NotFound</c>.</description></item>
    /// <item><description><c>401 Unauthorized</c> if any error is of type <c>Unauthorized</c>.</description></item>
    /// <item><description><c>403 Forbidden</c> if any error is of type <c>Forbidden</c>.</description></item>
    /// <item><description><c>400 BadRequest</c> if any error is of type <c>Invalid</c>.</description></item>
    /// <item><description><c>500 InternalServerError</c> if no specific error type matches.</description></item>
    /// </list>
    /// </returns>
    public static ActionResult MapErrorResult(this ControllerBase controller, Result result)
    {
        var errors = result.Errors!;
        if (errors.Any(e => e.Type == ErrorType.NotFound))
            return controller.NotFound(errors);
        if (errors.Any(e => e.Type == ErrorType.Unauthorized))
            return controller.Unauthorized(errors);
        if (errors.Any(e => e.Type == ErrorType.Forbidden))
            return controller.StatusCode(StatusCodes.Status403Forbidden, result.Errors);
        if (errors.Any(e => e.Type == ErrorType.Invalid))
            return controller.BadRequest(errors);
        return controller.StatusCode(500, errors);
    }
}