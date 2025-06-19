namespace Agora.Core.Models.Requests;

public class TransactionDetailsUpdate
{
    public string? Title { get; set; }
    public int? Price { get; set; }
    public long? PostId { get; set; }
    public DateOnly? TransactionDate { get; set; }
}