namespace Agora.Core.Models;

public class PostCategory : BaseEntity
{
    public required string Name { get; set; }
    public ICollection<Post> Posts { get; set; } = new List<Post>();
}