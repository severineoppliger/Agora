namespace Agora.Core.Models;

public class Transaction : BaseEntity
{
    public required int Price { get; set; }
        
    public required long? PostId { get; set; }
    public Post? Post { get; set; }
    
    public required long TransactionStatusId { get; set; }
    public TransactionStatus? TransactionStatus { get; set; }
    
    public required long BuyerId { get; set; }
    public User? Buyer { get; set; }
    
    public required long SellerId { get; set; }
    public User? Seller { get; set; }

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