using System.ComponentModel.DataAnnotations;
using Agora.Core.Validation;

namespace Agora.API.DTOs.TransactionStatus;

public class UpdateTransactionStatusDetailsDto
{
    [MinLength(ValidationRules.TransactionStatus.NameMinLength, ErrorMessage = "{0} must be at least {1} characters.")]
    [MaxLength(ValidationRules.TransactionStatus.NameMaxLength, ErrorMessage = "{0} must be less than {1} characters.")]
    public string? Name { get; set; }
    
    [MinLength(ValidationRules.TransactionStatus.DescriptionMinLength, ErrorMessage = "{0} must be at least {1} characters.")]
    [MaxLength(ValidationRules.TransactionStatus.DescriptionMaxLength, ErrorMessage = "{0} must be less than {1} characters.")]
    public string? Description { get; set; }
    
    public bool IsEmpty()
    {
        return Name == null
               && Description == null;
    }
}