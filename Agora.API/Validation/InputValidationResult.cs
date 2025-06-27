namespace Agora.API.Validation;


/// <summary>
/// Represents the result of input validation, containing any validation error messages.
/// </summary>
// Use this to accumulate validation errors when validating user input (DTOs, IDs, etc.)
public class InputValidationResult
{
    /// <summary>
    /// Gets the list of validation error messages.
    /// Each entry corresponds to one validation failure.
    /// </summary>
    public List<string> Errors { get; } = [];

    /// <summary>
    /// Gets a value indicating whether the input is valid (no validation errors).
    /// Returns <c>true</c> if there are no errors; otherwise, <c>false</c>.
    /// </summary>
    /// <returns><c>true</c> if there are no errors; otherwise, <c>false</c>.</returns>
    public bool IsValid => Errors.Count == 0;
}