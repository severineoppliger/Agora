namespace Agora.Core.Commands;

public class UpdateTransactionDetailsCommand
{
    public string? Title { get; set; }
    public int? Price { get; set; }
    public long? PostId { get; set; }
    public DateOnly? TransactionDate { get; set; }
}