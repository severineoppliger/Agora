using Agora.Core.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

namespace Agora.API.Filters;

// Filter (like a hook) to add logs related to actions methods in controllers
public class LogActionFilter(ILogger<LogActionFilter> logger) : IActionFilter
{
    // Before the action methods runs
    public void OnActionExecuting(ActionExecutingContext context)
    {
        logger.LogInformation("\u27a1  Entering action {ActionName} with parameters {Parameters}", 
            context.ActionDescriptor.DisplayName,
            context.ActionArguments.Values);
        
        if (!context.ModelState.IsValid)
        {
            Dictionary<string, List<string>> validationErrors = ModelStateExtensions.ExtractErrors(context.ModelState);
            logger.LogWarning("\u26A0  Data validation has failed for action {ActionName}. Errors: {@ValidationErrors}",
                context.ActionDescriptor.DisplayName,
                validationErrors);

            // Retourner un BadRequest personnalisé avec les détails d'erreur dans la réponse
            context.Result = new BadRequestObjectResult(context.ModelState);
        }
    }

    // After the action methods runs
    public void OnActionExecuted(ActionExecutedContext context)
    {
        // Extract return result of the action method
        switch (context.Result)
        {
            
            case BadRequestObjectResult badRequest:
            {
                var details = badRequest.Value;

                if (details is ValidationProblemDetails validationProblemDetails)
                {
                    Log.Warning("\u2b05  Exiting action {ActionName}. Result is BadRequest with errors: {@Errors}",
                        context.ActionDescriptor.DisplayName,
                        validationProblemDetails.Errors);
                }
                else
                {
                    Log.Warning("\u2b05  Exiting action {ActionName}. Result is BadRequest with details  {@Details}",
                        context.ActionDescriptor.DisplayName,
                        details);
                }

                break;
            }
            case ObjectResult result:
                logger.LogInformation("\u2b05  Exiting action {ActionName}: {StatusCode} - {Result}",
                    context.ActionDescriptor.DisplayName,
                    result.StatusCode,
                    result.Value);
                break;
            case StatusCodeResult statusResult:
                Log.Information("\u2b05  Exiting action {ActionName}: {StatusCode}",
                    context.ActionDescriptor.DisplayName,
                    statusResult.StatusCode);
                break;
            default:
                Log.Information("\u2b05  Exiting action {ActionName} with unknown result", 
                    context.ActionDescriptor.DisplayName);
                break;
        }
    }
}