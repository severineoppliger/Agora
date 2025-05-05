using Agora.API.DTOs.Post;

namespace Agora.API.DTOs.Transaction;

public class TransactionDetailsDto
{
    public long Id { get; set; }
    public int Price { get; set; }
    public PostSummaryDto Post { get; set; } = new();
    public string TransactionStatusName { get; set; } = String.Empty;
    public string BuyerUsername { get; set; } = String.Empty;
    public string SellerUsername { get; set; } = String.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    
    public override string ToString()
    {
        return $"Id: {Id}, " +
               $"Price: {Price}, " +
               $"Related post id: {Post.Id}, " +
               $"Transaction status: {TransactionStatusName}, " +
               $"Buyer username: {BuyerUsername}, " +
               $"Seller username: {SellerUsername}, " +
               $"CreatedAt: {CreatedAt:yyyy-MM-dd HH:mm:ss}, " +
               $"CompletedAt: {CompletedAt:yyyy-MM-dd HH:mm:ss}";
    }
}