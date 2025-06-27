using System.ComponentModel.DataAnnotations;

namespace Agora.API.Validation;

/// <summary>
/// Validation attribute that ensures a string is not null, empty, or consists only of whitespace characters.
/// Apply on a property or field using [NotEmptyOrWhitespace].
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class NotEmptyOrWhitespaceAttribute : ValidationAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NotEmptyOrWhitespaceAttribute"/> class.
    /// Sets the default error message.
    /// </summary>
    public NotEmptyOrWhitespaceAttribute()
    {
        ErrorMessage = "The {0} field cannot be empty or consist only of whitespace.";
    }

    /// <summary>
    /// Determines whether the specified value is valid (not null and not empty).
    /// </summary>
    /// <param name="value">The value of the field or property to validate.</param>
    /// <returns><c>true</c> if valid; otherwise, <c>false</c>.</returns>
    public override bool IsValid(object? value)
    {
        if (value is string str)
        {
            return !string.IsNullOrWhiteSpace(str);
        }

        return true;
    }
}