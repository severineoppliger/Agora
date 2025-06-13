using Agora.Core.Enums;

namespace Agora.Core.Models;

public class TransactionStatus : BaseEntity
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required bool IsFinal { get; set; }
    public required bool IsSuccess { get; set; }
    public required TransactionStatusEnum EnumValue { get; set; }
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}