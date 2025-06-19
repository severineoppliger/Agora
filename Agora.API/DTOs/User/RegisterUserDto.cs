using System.ComponentModel.DataAnnotations;
using Agora.API.Validation;
using Agora.Core.Validation;

namespace Agora.API.DTOs.User;

/// <summary>
/// Data Transfer Object used to register a new user.
/// Contains the required fields to create a new user account.
/// </summary>
public class RegisterUserDto
{
    /// <summary>
    /// Desired username for the new user.
    /// </summary>
    [Required]
    [NotEmptyOrWhitespace]
    [MinLength(ValidationConstants.User.UsernameMinLength, ErrorMessage = "{0} must be at least {1} characters.")]
    [MaxLength(ValidationConstants.User.UsernameMaxLength, ErrorMessage = "{0} must be less than {1} characters.")]
    public string UserName { get; set; } = String.Empty;
    
    /// <summary>
    /// Email address of the new user.
    /// </summary>
    [Required]
    [NotEmptyOrWhitespace]
    [EmailAddress]
    [MinLength(ValidationConstants.User.EmailMinLength, ErrorMessage = "{0} must be at least {1} characters.")]
    [MaxLength(ValidationConstants.User.EmailMaxLength, ErrorMessage = "{0} must be less than {1} characters.")]
    public string Email { get; set; } = String.Empty;
    
    /// <summary>
    /// Password for the new user account.
    /// </summary>
    [Required]
    [NotEmptyOrWhitespace]
    [MinLength(ValidationConstants.User.PasswordMinLength, ErrorMessage = "{0} must be at least {1} characters.")]
    [MaxLength(ValidationConstants.User.PasswordMaxLength, ErrorMessage = "{0} must be less than {1} characters.")]
    public string Password { get; set; } = String.Empty;
}