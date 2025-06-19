using System.ComponentModel.DataAnnotations;
using Agora.Core.Validation;

namespace Agora.API.DTOs.User;

/// <summary>
/// Data Transfer Object used to register a new user.
/// Contains the required fields to create a new user account.
/// </summary>
public class RegisterDto
{
    /// <summary>
    /// Desired username for the new user.
    /// </summary>
    [Required]
    [NotEmptyOrWhitespace]
    [MinLength(ValidationRules.AppUser.UsernameMinLength, ErrorMessage = "{0} must be at least {1} characters.")]
    [MaxLength(ValidationRules.AppUser.UsernameMaxLength, ErrorMessage = "{0} must be less than {1} characters.")]
    public string UserName { get; set; } = String.Empty;
    
    /// <summary>
    /// Email address of the new user.
    /// </summary>
    [Required]
    [NotEmptyOrWhitespace]
    [EmailAddress]
    [MinLength(ValidationRules.AppUser.EmailMinLength, ErrorMessage = "{0} must be at least {1} characters.")]
    [MaxLength(ValidationRules.AppUser.EmailMaxLength, ErrorMessage = "{0} must be less than {1} characters.")]
    public string Email { get; set; } = String.Empty;
    
    /// <summary>
    /// Password for the new user account.
    /// </summary>
    [Required]
    [NotEmptyOrWhitespace]
    [MinLength(ValidationRules.AppUser.PasswordMinLength, ErrorMessage = "{0} must be at least {1} characters.")]
    [MaxLength(ValidationRules.AppUser.PasswordMaxLength, ErrorMessage = "{0} must be less than {1} characters.")]
    public string Password { get; set; } = String.Empty;
}