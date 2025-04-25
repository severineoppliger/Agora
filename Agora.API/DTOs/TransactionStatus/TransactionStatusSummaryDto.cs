namespace Agora.API.DTOs.TransactionStatus;

public class TransactionStatusSummaryDto
{
    public long Id { get; set; }
    public string Name { get; set; } = String.Empty;
    public bool IsFinal { get; set; }
}