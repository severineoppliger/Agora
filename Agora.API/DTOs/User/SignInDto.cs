using System.ComponentModel.DataAnnotations;

namespace Agora.API.DTOs.User;

public class SignInDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = String.Empty;
    
    [Required]
    public string Password { get; set; } = String.Empty;
}