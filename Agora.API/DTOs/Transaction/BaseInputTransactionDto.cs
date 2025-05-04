using System.ComponentModel.DataAnnotations;
using Agora.Core.Validation;

namespace Agora.API.DTOs.Transaction;

public abstract class BaseInputTransactionDto
{
    [Range(ValidationRules.Transaction.PriceMin, ValidationRules.Transaction.PriceMax, ErrorMessage = "{0} must be between {1} and {2}.")]
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