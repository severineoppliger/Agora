using Agora.Core.Enums;

namespace Agora.Core.Common;

public class ErrorDetail(ErrorType type, string message)
{
    public ErrorType Type { get; } = type;
    public string Message { get; } = message;

    public override string ToString() => $"{Type}: {Message}";
}