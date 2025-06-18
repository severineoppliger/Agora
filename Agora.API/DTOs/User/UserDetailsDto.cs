using Agora.API.DTOs.Post;
using Agora.API.DTOs.Transaction;

namespace Agora.API.DTOs.User;

/// <summary>
/// Data Transfer Object used for controller output.
/// Represents the full details of a user, including profile information, credit balance, posts and transactions.
/// </summary>
public class UserDetailsDto
{
    /// <summary>
    /// Unique identifier of the user.
    /// </summary>
    public string Id { get; set; } = String.Empty;
    
    /// <summary>
    /// Username of the user.
    /// </summary>
    public string UserName { get; set; } = String.Empty;
    
    /// <summary>
    /// Email address of the user.
    /// </summary>
    public string Email { get; set; } = String.Empty;
    
    /// <summary>
    /// Current credit balance of the user (in Kairos credits).
    /// </summary>
    public int Credit { get; set; }
    
    /// <summary>
    /// Date and time when the user account was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// Date and time of the user's last login.
    /// </summary>
    public DateTime LastLoginAt { get; set; }

    /// <summary>
    /// List of posts created by the user.
    /// </summary>
    public List<PostSummaryDto> Posts { get; set; } = new();
    
    /// <summary>
    /// List of transactions where the user acted as a buyer.
    /// </summary>
    public List<TransactionSummaryDto> TransactionsAsBuyer { get; set; } = new();
    
    /// <summary>
    /// List of transactions where the user acted as a seller.
    /// </summary>
    public List<TransactionSummaryDto> TransactionsAsSeller { get; set; } = new();
    
    /// <inheritdoc/>
    public override string ToString()
    {
        return $"Id: {Id}, Username: {UserName}, Email: {Email}, Credit: {Credit}, " +
               $"CreatedAt: {CreatedAt:yyyy-MM-dd HH:mm:ss}, LastLoginAt: {LastLoginAt:yyyy-MM-dd HH:mm:ss}, " +
               $"Nb of posts: {Posts.Count}, Nb of transactions as buyer: {TransactionsAsBuyer.Count}, " +
               $"Nb of transactions as seller: {TransactionsAsSeller.Count}";
    }
}