namespace Agora.API.DTOs.Post;

/// <summary>
/// Data Transfer Object used for controller output.
/// It represents a summary view of a <c>Post</c>, typically used in lists or search results.
/// </summary>
public class PostSummaryDto
{
    /// <summary>
    /// Unique identifier of the post.
    /// </summary>
    public long Id { get; set; }
    
    /// <summary>
    /// Title of the post.
    /// </summary>
    public string Title { get; set; } = string.Empty;
    
    /// <summary>
    /// Price of the item or service in credits (Kairos).
    /// </summary>
    public int Price { get; set; }
    
    /// <summary>
    /// Type name of the post ('Offer' or 'Request').
    /// </summary>
    public string TypeName { get; set; } = string.Empty;
    
    /// <summary>
    /// Status name of the post ('Active', 'Inactive' or 'Deleted').
    /// </summary>
    public string StatusName { get; set; } = string.Empty;
    
    /// <summary>
    /// Name of the post category in which the post is classified.
    /// </summary>
    public string PostCategoryName { get; set; } = string.Empty;
    
    /// <summary>
    /// Username of the post author.
    /// </summary>
    public string UserName { get; set; } = string.Empty;
}