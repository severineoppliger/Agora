namespace Agora.API.DTOs.User;

public class CreateUserDto
{
    public required string Username { get; set; }
    public required string Email { get; set; }
    
    // Raw password, as the hash is created server-side
    public required string Password { get; set; } 
}