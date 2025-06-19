namespace Agora.API.DTOs.PostCategory;

/// <summary>
/// Data Transfer Object used for controller output.
/// It represents a summary view of a <c>PostCategory</c>, typically used in lists or search results.
/// </summary>
public class PostCategorySummaryDto
{
    /// <summary>
    /// Unique identifier of the post category.
    /// </summary>
    public long Id { get; set; }
    
    /// <summary>
    /// Name of the post category.
    /// </summary>
    public string Name { get; set; } = String.Empty;
}