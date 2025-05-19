using System.ComponentModel.DataAnnotations;

namespace Agora.API.DTOs.Account;

public class RegisterDto
{
    [Required]
    public string UserName { get; set; } = String.Empty;
    
    [Required]
    [EmailAddress]
    public string Email { get; set; } = String.Empty;
    
    [Required]
    public string Password { get; set; } = String.Empty;
}