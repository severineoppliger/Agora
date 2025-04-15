using Agora.Core.Enums;

namespace Agora.Core.Models;

public class Post : BaseEntity
{
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required int Price { get; set; }
    public required PostType Type { get; set; }
    public required PostStatus Status { get; set; } = PostStatus.Draft;
        
    public long PostCategoryId { get; set; }
    public required PostCategory PostCategory { get; set; }

    public long UserId { get; set; }
    public required User User { get; set; }

    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    public required DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}