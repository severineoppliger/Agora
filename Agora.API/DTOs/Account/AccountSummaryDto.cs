namespace Agora.API.DTOs.Account;

public class AccountSummaryDto
{
    public long Id { get; set; }
    public string UserName { get; set; } = String.Empty;
    public string Email { get; set; } = String.Empty;
    public int Credit { get; set; }
}