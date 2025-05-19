using Microsoft.AspNetCore.Identity;

namespace Agora.Core.Models;

public class AppUser : IdentityUser
{
    public int Credit { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastLoginAt { get; set; }
    
    public ICollection<Post> Posts { get; set; } = new List<Post>();
    public ICollection<Transaction> TransactionsAsBuyer { get; set; } = new List<Transaction>();
    public ICollection<Transaction> TransactionsAsSeller { get; set; } = new List<Transaction>();
}