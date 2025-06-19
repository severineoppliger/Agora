namespace Agora.Core.Extensions;

public static class StringExtensions
{
    public static bool IsGuid(this string? input)
    {
        return Guid.TryParse(input, out _);
    }
    
    public static string CapitalizeFirstLetter(this string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        return char.ToUpper(input[0]) + input.Substring(1);
    }
}