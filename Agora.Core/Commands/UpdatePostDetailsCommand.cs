namespace Agora.Core.Commands;

/// <summary>
/// Command to update details of an existing <c>Post</c>.
/// Only provided properties will be updated; null values will be ignored.
/// </summary>
public class UpdatePostDetailsCommand
{
    /// <summary>
    /// New title of the post.
    /// </summary>
    public string? Title { get; set; }
    
    /// <summary>
    /// New description of the post.
    /// </summary>
    public string? Description { get; set; }
    
    /// <summary>
    /// New price of the item or service in credits (Kairos).
    /// </summary>
    public int? Price { get; set; }
    
    /// <summary>
    /// New type of the post (should have values 'Offer' or 'Request').
    /// </summary>
    public string? Type { get; set; }
    
    /// <summary>
    /// Identifier of the new post category in which the post is classified.
    /// </summary>
    public long? PostCategoryId { get; set; }
}