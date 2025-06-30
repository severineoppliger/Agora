using System.ComponentModel.DataAnnotations;
using Agora.Core.Validation;

namespace Agora.API.DTOs.TransactionStatus;

/// <summary>
/// Data Transfer Object for updating an existing <c>TransactionStatus</c>.
/// </summary>
public class UpdateTransactionStatusDetailsDto
{
    /// <summary>
    /// New name of the transaction status (in French).
    /// </summary>
    [MinLength(ValidationConstants.TransactionStatus.NameMinLength, ErrorMessage = "{0} must be at least {1} characters.")]
    [MaxLength(ValidationConstants.TransactionStatus.NameMaxLength, ErrorMessage = "{0} must be less than {1} characters.")]
    public string? Name { get; set; }
    
    /// <summary>
    /// New description of the transaction status.
    /// </summary>
    [MinLength(ValidationConstants.TransactionStatus.DescriptionMinLength, ErrorMessage = "{0} must be at least {1} characters.")]
    [MaxLength(ValidationConstants.TransactionStatus.DescriptionMaxLength, ErrorMessage = "{0} must be less than {1} characters.")]
    public string? Description { get; set; }
    
    /// <summary>
    /// Determines whether the current object is considered empty.
    /// An object is considered empty if all its properties are <c>null</c>.
    /// </summary>
    /// <returns>
    /// <c>true</c> if all its properties are <c>null</c>; otherwise, <c>false</c>.
    /// </returns>
    public bool IsEmpty()
    {
        return Name == null && Description == null;
    }
}