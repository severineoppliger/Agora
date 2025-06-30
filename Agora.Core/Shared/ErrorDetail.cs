using Agora.Core.Enums;

namespace Agora.Core.Shared;

/// <summary>
/// Represents a detailed error with a type and a descriptive message.
/// </summary>
/// <param name="type">The type of the error.</param>
/// <param name="message">The descriptive error message.</param>
public class ErrorDetail(ErrorType type, string message)
{
    /// <summary>
    /// The type of the error.
    /// </summary>
    public ErrorType Type { get; } = type;
    
    /// <summary>
    /// The descriptive error message.
    /// </summary>
    public string Message { get; } = message;

    /// <summary>
    /// Returns a string representation of the error detail.
    /// </summary>
    public override string ToString() => $"{Type}: {Message}";
}