using Agora.API.DTOs.Transaction;

namespace Agora.API.DTOs.TransactionStatus;

public class TransactionStatusDetailsDto
{
    public long Id { get; set; }
    public string Name { get; set; } = String.Empty;
    public string Description { get; set; } = String.Empty;
    public bool IsFinal { get; set; }
    public bool IsSuccess { get; set; }
    public ICollection<TransactionSummaryDto> Transactions { get; set; } = new List<TransactionSummaryDto>();
}