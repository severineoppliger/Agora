namespace Agora.API.DTOs.TransactionStatus;

public class TransactionStatusSummaryDto
{
    public string Name { get; set; } = String.Empty;
    public string Description { get; set; } = String.Empty;
    public bool IsFinal { get; set; }
    public bool IsSuccess { get; set; }
}