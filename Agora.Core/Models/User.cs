namespace Agora.Core.Models;

public class User : BaseEntity
{
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public required DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime LastLoginAt { get; set; } = DateTime.UtcNow;
    public required int Credit { get; set; }
    public ICollection<Post> Posts { get; set; } = new List<Post>();
    public ICollection<Transaction> TransactionsAsBuyer { get; set; } = new List<Transaction>();
    public ICollection<Transaction> TransactionsAsSeller { get; set; } = new List<Transaction>();
}