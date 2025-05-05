using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Agora.Core.Extensions;

public static class ModelStateExtensions
{
    // Extract the errors for all fields not compliant with data annotations (in DTO or model)
    public static Dictionary<string, List<string>> ExtractErrors(ModelStateDictionary modelState)
    {
        return modelState
            .Where(ms => ms.Value != null && ms.Value.Errors.Any())
            .ToDictionary(
                ms => ms.Key,
                ms => ms.Value!.Errors.Select(e => e.ErrorMessage).ToList()
            );
    }
}