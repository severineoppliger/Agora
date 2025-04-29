namespace Agora.Core.Models;

public class Transaction : BaseEntity
{
    public required int Price { get; set; }
        
    public long? PostId { get; set; }
    public required Post? Post { get; set; }
    
    public long TransactionStatusId { get; set; }
    public required TransactionStatus? TransactionStatus { get; set; }
    
    public long BuyerId { get; set; }
    public required User? Buyer { get; set; }
    
    public long SellerId { get; set; }
    public required User? Seller { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }
    
    public void Deconstruct(out int price, out Post? post, out long buyerId, out User? buyer, out long sellerId)
    {
        price = Price;
        post = Post;
        buyerId = BuyerId;
        buyer = Buyer;
        sellerId = SellerId;
    }
}