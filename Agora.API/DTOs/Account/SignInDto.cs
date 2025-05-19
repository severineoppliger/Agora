using System.ComponentModel.DataAnnotations;

namespace Agora.API.DTOs.Account;

public class SignInDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = String.Empty;
    
    [Required]
    public string Password { get; set; } = String.Empty;
}