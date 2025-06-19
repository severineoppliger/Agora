using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Agora.Core.Extensions;

/// <summary>
/// Provides extension methods for <see cref="ModelStateDictionary"/> to simplify error extraction from data annotations.
/// </summary>
public static class ModelStateExtensions
{
    /// <summary>
    /// Extracts validation errors from the model state as a dictionary.
    /// </summary>
    /// <param name="modelState">The model state containing validation errors.</param>
    /// <returns>
    /// A dictionary where each key is a field name and each value is a list of error messages associated with that field.
    /// </returns>
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