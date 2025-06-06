namespace Agora.API.DTOs.User;

public class UserSummaryDto
{
    public string Id { get; set; } = String.Empty;
    public string UserName { get; set; } = String.Empty;
    public string Email { get; set; } = String.Empty;
    public int Credit { get; set; }
}