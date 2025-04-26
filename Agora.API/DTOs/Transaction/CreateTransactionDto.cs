namespace Agora.API.DTOs.Transaction;

public class CreateTransactionDto
{
    public required int Price { get; set; }
    public required long PostId { get; set; }
    public long TransactionStatusId { get; set; }
    public long BuyerId { get; set; }
    public long SellerId { get; set; }
}