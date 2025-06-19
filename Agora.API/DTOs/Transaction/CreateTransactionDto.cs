using System.ComponentModel.DataAnnotations;
using Agora.API.Validation;
using Agora.Core.Validation;

namespace Agora.API.DTOs.Transaction;

/// <summary>
/// Data Transfer Object for creating a new <c>Transaction</c>. 
/// Contains the necessary information such as title, price, buyer and seller identifiers, 
/// and optional details like related post and transaction date.
/// </summary>
public class CreateTransactionDto
{
    /// <summary>
    /// Title of the transaction.
    /// </summary>
    [Required]
    [NotEmptyOrWhitespace]
    [MinLength(ValidationConstants.Transaction.TitleMinLength, ErrorMessage = "{0} must be at least {1} characters.")]
    [MaxLength(ValidationConstants.Transaction.TitleMaxLength, ErrorMessage = "{0} must be less than {1} characters.")]
    public string Title { get; set; } = String.Empty;
    
    /// <summary>
    /// Price of the transaction in Kairos credits.
    /// </summary>
    [Required]
    [Range(ValidationConstants.Transaction.PriceMin, ValidationConstants.Transaction.PriceMax, ErrorMessage = "{0} must be between {1} and {2}.")]
    public int Price { get; set; }
    
    /// <summary>
    /// Optional ID of the related post, if the transaction is linked to a published post.
    /// </summary>
    public long? PostId { get; set; }
    
    /// <summary>
    /// Optional transaction date (format "yyyy-MM-dd"). 
    /// </summary>
    [DataType(DataType.Date)]
    public DateTime? TransactionDate { get; set; }
    
    /// <summary>
    /// Identifier of the user acting as buyer in the transaction.
    /// </summary>
    [Required]
    public string BuyerId { get; set; } = String.Empty;
    
    /// <summary>
    /// Identifier of the user acting as seller in the transaction.
    /// </summary>
    [Required]
    public string SellerId { get; set; } = String.Empty;

}