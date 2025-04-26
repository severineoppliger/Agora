namespace Agora.API.DTOs.TransactionStatus;

public class CreateTransactionStatusDto
{
    public string Name { get; set; } = String.Empty;
    public bool IsFinal { get; set; }
}