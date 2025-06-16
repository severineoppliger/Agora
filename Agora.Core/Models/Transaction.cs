namespace Agora.Core.Models;

public class Transaction : BaseEntity
{
    public required string Title { get; set; }
    public required int Price { get; set; }
        
    public required long? PostId { get; set; }
    public Post? Post { get; set; }
    
    public required long TransactionStatusId { get; set; }
    public TransactionStatus? TransactionStatus { get; set; }
    
    public required string InitiatorId { get; set; }
    public required string BuyerId { get; set; }
    public required bool BuyerConfirmed { get; set; }
    public User? Buyer { get; set; }
    
    public required string SellerId { get; set; }
    public required bool SellerConfirmed { get; set; }
    public User? Seller { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public DateOnly? TransactionDate { get; set; }
    public DateTime? CompletedAt { get; set; }
    
    public void Deconstruct(out int price, out Post? post, out string buyerId, out User? buyer, out string sellerId)
    {
        price = Price;
        post = Post;
        buyerId = BuyerId;
        buyer = Buyer;
        sellerId = SellerId;
    }
}