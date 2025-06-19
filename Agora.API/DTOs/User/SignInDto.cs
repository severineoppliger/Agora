using System.ComponentModel.DataAnnotations;

namespace Agora.API.DTOs.User;

/// <summary>
/// Data Transfer Object used to authenticate a user (sign in).
/// Contains the required credentials for authentication.
/// </summary>
public class SignInDto
{
    /// <summary>
    /// Email address of the user attempting to sign in.
    /// </summary>
    [Required]
    [EmailAddress]
    public string Email { get; set; } = String.Empty;
    
    /// <summary>
    /// Password of the user attempting to sign in.
    /// </summary>
    [Required]
    public string Password { get; set; } = String.Empty;
}