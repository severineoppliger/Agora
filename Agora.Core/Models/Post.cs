using Agora.Core.Enums;

namespace Agora.Core.Models;

/// <summary>
/// Entity representing a published post within the system.
/// Maps to the corresponding database table via EF Core.
/// A post includes its content, type, category, owner, related transactions, and metadata.
/// </summary>
public class Post : BaseEntity
{
    /// <summary>
    /// Title of the post.
    /// </summary>
    public required string Title { get; set; }
    
    /// <summary>
    /// Description of the post.
    /// </summary>
    public required string Description { get; set; }
    
    /// <summary>
    /// Price of the item or service in credits (Kairos).
    /// </summary>
    public required int Price { get; set; }
    
    /// <summary>
    /// Type of the post (offer or request).
    /// </summary>
    public required PostType Type { get; set; }
    
    /// <summary>
    /// Current status of the post (active, inactive, etc.).
    /// </summary>
    public required PostStatus Status { get; set; } = PostStatus.Inactive;
        
    /// <summary>
    /// Foreign key referencing the associated <c>PostCategory</c>'s unique identifier <c>Id</c>.
    /// </summary>
    public required long PostCategoryId { get; set; }
    
    /// <summary>
    /// Navigation property that refers to the <c>PostCategory</c> of this post.
    /// </summary>
    public PostCategory PostCategory { get; set; }
    
    /// <summary>
    /// Foreign key referencing the associated <c>User</c> as the owner of the post.
    /// It corresponds to a user's unique identifier <c>Id</c>.
    /// </summary>
    public required string OwnerUserId { get; set; }
    
    /// <summary>
    /// Navigation property to the user who owns the post.
    /// </summary>
    public User Owner { get; set; }

    /// <summary>
    /// List of transactions related to this post.
    /// </summary>
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    /// <summary>
    /// Date and time (UTC) when the post was created.
    /// </summary>
    public required DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// Date and time (UTC) when the post was last updated, if applicable.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}