using System.ComponentModel.DataAnnotations;
using Agora.Core.Validation;

namespace Agora.API.DTOs.Transaction;

/// <summary>
/// Data Transfer Object for updating an existing <c>Transaction</c> properties.
/// </summary>
public class UpdateTransactionDetailsDto
{
    /// <summary>
    /// New title of the transaction.
    /// </summary>
    [MinLength(ValidationConstants.Transaction.TitleMinLength, ErrorMessage = "{0} must be at least {1} characters.")]
    [MaxLength(ValidationConstants.Transaction.TitleMaxLength, ErrorMessage = "{0} must be less than {1} characters.")]
    public string? Title { get; set; }

    /// <summary>
    /// New price of the transaction in Kairos credits.
    /// </summary>
    [Range(ValidationConstants.Transaction.PriceMin, ValidationConstants.Transaction.PriceMax, ErrorMessage = "{0} must be between {1} and {2}.")]
    public int? Price { get; set; }
    
    /// <summary>
    /// Optional ID of the new related post, if the transaction is linked to a published post.
    /// </summary>
    public long? PostId { get; set; }
    
    /// <summary>
    /// New optional transaction date (format "yyyy-MM-dd"). 
    /// </summary>
    [DataType(DataType.Date)]
    public DateTime? TransactionDate { get; set; }
    
    /// <summary>
    /// Determines whether the current object is considered empty.
    /// An object is considered empty if all its properties are <c>null</c>.
    /// </summary>
    /// <returns>
    /// <c>true</c> if all its properties are <c>null</c>; otherwise, <c>false</c>.
    /// </returns>
    public bool IsEmpty()
    {
        return Title == null
               && Price == null
               && PostId == null
               && TransactionDate == null;
    }
}