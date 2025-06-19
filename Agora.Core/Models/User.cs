using Microsoft.AspNetCore.Identity;

namespace Agora.Core.Models;

/// <summary>
/// Entity representing an application user.
/// Inherits from <c>IdentityUser</c> (having properties like <c>UserName</c>, <c>Email</c> and <c>PasswordHash</c>
/// and extends it with domain-specific properties.
/// </summary>
public class User : IdentityUser
{
    /// <summary>
    /// Current credit balance of the user (in Kairos credits).
    /// </summary>
    public int Credit { get; set; }
    
    /// <summary>
    /// Date and time (UTC) when the user account was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// Date and time (UTC) of the user's last login, if applicable.
    /// </summary>
    public DateTime? LastLoginAt { get; set; }
    
    /// <summary>
    /// Navigation property that represents the collection of <c>Post</c> entities authored by the user.
    /// </summary>
    public ICollection<Post> Posts { get; set; } = new List<Post>();
    
    /// <summary>
    /// Navigation property that represents the collection of <c>Transaction</c> entities where the user is the buyer.
    /// </summary>
    public ICollection<Transaction> TransactionsAsBuyer { get; set; } = new List<Transaction>();
    
    /// <summary>
    /// Navigation property that represents the collection of <c>Transaction</c> entities where the user is the seller.
    /// </summary>
    public ICollection<Transaction> TransactionsAsSeller { get; set; } = new List<Transaction>();
}