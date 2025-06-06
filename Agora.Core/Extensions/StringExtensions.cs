namespace Agora.Core.Extensions;

public static class StringExtensions
{
    public static bool IsGuid(this string? input)
    {
        return Guid.TryParse(input, out _);
    }
}