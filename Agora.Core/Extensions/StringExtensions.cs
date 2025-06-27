namespace Agora.Core.Extensions;

/// <summary>
/// Provides extension methods for the <see cref="string"/> type.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Determines whether the specified string is a valid GUID.
    /// </summary>
    /// <param name="input">The input string to check.</param>
    /// <returns>
    /// <c>true</c> if the input is a valid GUID; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsGuid(this string? input)
    {
        return Guid.TryParse(input, out _);
    }
    
    /// <summary>
    /// Capitalizes the first letter of the input string.
    /// </summary>
    /// <param name="input">The input string.</param>
    /// <returns>
    /// A new string with the first letter capitalized. 
    /// If the input is null or empty, the original input is returned.
    /// </returns>
    public static string CapitalizeFirstLetter(this string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        return char.ToUpper(input[0]) + input.Substring(1);
    }
}