using System.ComponentModel.DataAnnotations;
using Agora.Core.Enums;
using Agora.Core.Validation;

namespace Agora.API.DTOs.Transaction;

public class UpdateTransactionDetailsDto
{
    [MinLength(ValidationRules.Transaction.TitleMinLength, ErrorMessage = "{0} must be at least {1} characters.")]
    [MaxLength(ValidationRules.Transaction.TitleMaxLength, ErrorMessage = "{0} must be less than {1} characters.")]
    public string? Title { get; set; } = String.Empty;

    [Range(ValidationRules.Transaction.PriceMin, ValidationRules.Transaction.PriceMax, ErrorMessage = "{0} must be between {1} and {2}.")]
    public int? Price { get; set; }
    
    public long? PostId { get; set; }
    
    // DateOnly is not supported so we need a Date like "2025-06-06"
    [DataType(DataType.Date)]
    public DateTime? TransactionDate { get; set; }
}