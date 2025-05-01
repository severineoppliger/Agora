namespace Agora.API.DTOs.TransactionStatus;

public abstract class BaseInputTransactionStatusDto
{
    public required string Name { get; set; } = String.Empty;
    public required string Description { get; set; } = String.Empty;
    public bool IsFinal { get; set; }
    public bool IsSuccess { get; set; }
}