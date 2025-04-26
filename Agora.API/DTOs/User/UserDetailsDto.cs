using Agora.API.DTOs.Post;
using Agora.API.DTOs.Transaction;

namespace Agora.API.DTOs.User;

public class UserDetailsDto
{
    public long Id { get; set; }
    public string Username { get; set; } = String.Empty;
    public string Email { get; set; } = String.Empty;
    public int Credit { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastLoginAt { get; set; }

    public List<PostSummaryDto> Posts { get; set; } = new();
    public List<TransactionSummaryDto> TransactionsAsBuyer { get; set; } = new();
    public List<TransactionSummaryDto> TransactionsAsSeller { get; set; } = new();
}