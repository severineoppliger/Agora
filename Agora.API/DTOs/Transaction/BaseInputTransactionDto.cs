namespace Agora.API.DTOs.Transaction;

public abstract class BaseInputTransactionDto
{
    public required int Price { get; set; }
    public required long? PostId { get; set; }
    public required long TransactionStatusId { get; set; }
    public required long BuyerId { get; set; }
    public required long SellerId { get; set; }
    
    public void Deconstruct(out int price, out long? postId, out long transactionStatusId, out long buyerId, out long sellerId)
    {
        price = Price;
        postId = PostId;
        transactionStatusId = TransactionStatusId;
        buyerId = BuyerId;
        sellerId = SellerId;
    }
}