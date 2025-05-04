using System.ComponentModel.DataAnnotations;
using Agora.Core.Validation;

namespace Agora.API.DTOs.TransactionStatus;

public abstract class BaseInputTransactionStatusDto
{
    [NotEmptyOrWhitespace]
    [MinLength(ValidationRules.TransactionStatus.NameMinLength, ErrorMessage = "{0} must be at least {1} characters.")]
    [MaxLength(ValidationRules.TransactionStatus.NameMaxLength, ErrorMessage = "{0} must be less than {1} characters.")]
    public required string Name { get; set; } = String.Empty;
    
    [NotEmptyOrWhitespace]
    [MinLength(ValidationRules.TransactionStatus.DescriptionMinLength, ErrorMessage = "{0} must be at least {1} characters.")]
    [MaxLength(ValidationRules.TransactionStatus.DescriptionMaxLength, ErrorMessage = "{0} must be less than {1} characters.")]
    public required string Description { get; set; } = String.Empty;
    
    public bool IsFinal { get; set; }
    
    public bool IsSuccess { get; set; }
}