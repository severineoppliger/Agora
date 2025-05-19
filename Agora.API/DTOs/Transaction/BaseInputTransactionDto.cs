using System.ComponentModel.DataAnnotations;
using Agora.Core.Validation;

namespace Agora.API.DTOs.Transaction;

public abstract class BaseInputTransactionDto
{
    [Required]
    [Range(ValidationRules.Transaction.PriceMin, ValidationRules.Transaction.PriceMax, ErrorMessage = "{0} must be between {1} and {2}.")]
    public int Price { get; set; }
    
    [Required]
    public long? PostId { get; set; }
    
    [Required]
    public long TransactionStatusId { get; set; }
    
    [Required]
    public long BuyerId { get; set; }
    
    [Required]
    public long SellerId { get; set; }
    
    public void Deconstruct(out int price, out long? postId, out long transactionStatusId, out long buyerId, out long sellerId)
    {
        price = Price;
        postId = PostId;
        transactionStatusId = TransactionStatusId;
        buyerId = BuyerId;
        sellerId = SellerId;
    }
}