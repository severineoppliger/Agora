using Agora.API.DTOs.Post;
using Agora.API.DTOs.Transaction;

namespace Agora.API.DTOs.AppUser;

public class AccountDetailsDto
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
    
    public override string ToString()
    {
        return $"Id: {Id}, Username: {Username}, Email: {Email}, Credit: {Credit}, " +
               $"CreatedAt: {CreatedAt:yyyy-MM-dd HH:mm:ss}, LastLoginAt: {LastLoginAt:yyyy-MM-dd HH:mm:ss}, " +
               $"Nb of posts: {Posts.Count}, Nb of transactions as buyer: {TransactionsAsBuyer.Count}, " +
               $"Nb of transactions as seller: {TransactionsAsSeller.Count}";
    }
}