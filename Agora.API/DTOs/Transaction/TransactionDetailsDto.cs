using Agora.API.DTOs.Post;

namespace Agora.API.DTOs.Transaction;

public class TransactionDetailsDto
{
    public int Price { get; set; }
    public PostSummaryDto Post { get; set; } = new();
    public string TransactionStatusName { get; set; } = String.Empty;
    public string BuyerUsername { get; set; } = String.Empty;
    public string SellerUsername { get; set; } = String.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
}