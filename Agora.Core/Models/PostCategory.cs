namespace Agora.Core.Models;

/// <summary>
/// Entity representing a category for posts.
/// Used to organize and classify posts within the system.
/// </summary>
public class PostCategory : BaseEntity
{
    /// <summary>
    /// Name of the post category.
    /// </summary>
    public required string Name { get; set; }
    
    /// <summary>
    /// Navigation property that represents the collection of <c>Post</c> entities in this post category.
    /// </summary>
    public ICollection<Post> Posts { get; set; } = new List<Post>();
}