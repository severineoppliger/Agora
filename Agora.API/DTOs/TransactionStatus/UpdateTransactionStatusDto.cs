namespace Agora.API.DTOs.TransactionStatus;

public class UpdateTransactionStatusDto
{
    public string Name { get; set; } = String.Empty;
    public bool IsFinal { get; set; }
}