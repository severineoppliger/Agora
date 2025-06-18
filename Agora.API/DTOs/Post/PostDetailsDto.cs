using Agora.API.DTOs.Transaction;

namespace Agora.API.DTOs.Post;

/// <summary>
/// Data Transfer Object used for controller output.
/// It represents the full details of a <c>Post</c>, including description, price, category, author and other related information.
/// </summary>
public class PostDetailsDto
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
    /// Description of the post.
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
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
    
    /// <summary>
    /// Date and time (UTC) when the post was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// Date and time (UTC) when the post was last updated.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
    
    /// <summary>
    /// List of transactions to which the post is linked.
    /// </summary>
    public List<TransactionSummaryDto> Transactions { get; set; } = new();
    
    /// <inheritdoc/>
    public override string ToString()
    {
        return $"Id: {Id}, Title: {Title}, Description: {Description}, CreatedAt: {CreatedAt:yyyy-MM-dd HH:mm:ss}";
    }
}