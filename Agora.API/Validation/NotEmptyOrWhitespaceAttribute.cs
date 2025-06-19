using System.ComponentModel.DataAnnotations;

namespace Agora.API.Validation;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class NotEmptyOrWhitespaceAttribute : ValidationAttribute
{
    public NotEmptyOrWhitespaceAttribute()
    {
        ErrorMessage = "The {0} field cannot be empty or consist only of whitespace.";
    }

    public override bool IsValid(object? value)
    {
        if (value is string str)
        {
            return !string.IsNullOrWhiteSpace(str);
        }

        return true;
    }
}