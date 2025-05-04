using System.ComponentModel.DataAnnotations;
using Agora.Core.Validation;

namespace Agora.API.DTOs.User;

public class CreateUserDto
{
    [NotEmptyOrWhitespace]
    [MinLength(ValidationRules.User.UsernameMinLength, ErrorMessage = "{0} must be at least {1} characters.")]
    [MaxLength(ValidationRules.User.UsernameMaxLength, ErrorMessage = "{0} must be less than {1} characters.")]
    public required string Username { get; set; }
    
    [EmailAddress]
    [NotEmptyOrWhitespace]
    [MinLength(ValidationRules.User.EmailMinLength, ErrorMessage = "{0} must be at least {1} characters.")]
    [MaxLength(ValidationRules.User.EmailMaxLength, ErrorMessage = "{0} must be less than {1} characters.")]
    public required string Email { get; set; }
    
    // Raw password, as the hash is created server-side
    [NotEmptyOrWhitespace]
    [MinLength(ValidationRules.User.PasswordMinLength, ErrorMessage = "{0} must be at least {1} characters.")]
    [MaxLength(ValidationRules.User.PasswordMaxLength, ErrorMessage = "{0} must be less than {1} characters.")]
    public required string Password { get; set; } 
}