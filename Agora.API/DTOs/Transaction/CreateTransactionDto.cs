using System.ComponentModel.DataAnnotations;
using Agora.Core.Validation;

namespace Agora.API.DTOs.Transaction;

public class CreateTransactionDto : BaseInputTransactionDto
{
    [Required]
    [NotEmptyOrWhitespace]
    [MinLength(ValidationRules.Transaction.TitleMinLength, ErrorMessage = "{0} must be at least {1} characters.")]
    [MaxLength(ValidationRules.Transaction.TitleMaxLength, ErrorMessage = "{0} must be less than {1} characters.")]
    public string Title { get; set; } = String.Empty;
    
    [Required]
    public string BuyerId { get; set; }
    
    [Required]
    public string SellerId { get; set; }
    
    public void Deconstruct(out string title, out int price, out long? postId, out string buyerId, out string sellerId, out DateTime? transactionDate)
    {
        title = Title;
        price = Price;
        postId = PostId;
        buyerId = BuyerId;
        sellerId = SellerId;
        transactionDate = TransactionDate;
    }
}