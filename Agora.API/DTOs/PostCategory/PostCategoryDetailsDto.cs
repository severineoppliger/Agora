using Agora.API.DTOs.Post;

namespace Agora.API.DTOs.PostCategory;

/// <summary>
/// Data Transfer Object used for controller output.
/// It represents the full details of a <c>PostCategory</c>, including the posts in this post category.
/// </summary>
public class PostCategoryDetailsDto
{
    /// <summary>
    /// Unique identifier of the post category.
    /// </summary>
    public long Id { get; set; }
    
    /// <summary>
    /// Name of the post category.
    /// </summary>
    public string Name { get; set; } = String.Empty;
    
    /// <summary>
    /// All posts in this post category.
    /// </summary>
    public List<PostSummaryDto> Posts { get; set; } = new();
    
    /// <inheritdoc/>
    public override string ToString()
    {
        return $"Id: {Id}, Name: {Name}, Nb of Posts: {Posts.Count}";
    }
}