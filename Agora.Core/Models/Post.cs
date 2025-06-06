using Agora.Core.Enums;

namespace Agora.Core.Models;

public class Post : BaseEntity
{
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required int Price { get; set; }
    public required PostType Type { get; set; }
    public required PostStatus Status { get; set; } = PostStatus.Inactive;
        
    public required long PostCategoryId { get; set; }
    public PostCategory PostCategory { get; set; }

    public required string OwnerUserId { get; set; }
    public AppUser Owner { get; set; }

    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    public required DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}