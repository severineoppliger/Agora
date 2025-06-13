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
    
    // DateOnly is not supported so we need a Date like "2025-06-06"
    [DataType(DataType.Date)]
    public DateTime? TransactionDate { get; set; }
    
    public void Deconstruct(out int price, out long? postId,out DateTime? transactionDate)
    {
        price = Price;
        postId = PostId;
        transactionDate = TransactionDate;
    }
}