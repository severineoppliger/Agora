namespace Agora.API.DTOs.User;

public class UserSummaryDto
{
    public long Id { get; set; }
    public string Username { get; set; } = String.Empty;
    public string Email { get; set; } = String.Empty;
    public int Credit { get; set; }
}