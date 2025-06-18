using System.ComponentModel.DataAnnotations;
using Agora.Core.Validation;

namespace Agora.API.DTOs.Post;

/// <summary>
/// Data Transfer Object for updating an existing <c>Post</c> properties.
/// </summary>
public class UpdatePostDetailsDto
{
    /// <summary>
    /// New title of the post.
    /// </summary>
    [MinLength(ValidationRules.Post.TitleMinLength, ErrorMessage = "{0} must be at least {1} characters.")]
    [MaxLength(ValidationRules.Post.TitleMaxLength, ErrorMessage = "{0} must be less than {1} characters.")]
    public string? Title { get; set; }
    
    /// <summary>
    /// New description of the post.
    /// </summary>
    [MinLength(ValidationRules.Post.DescriptionMinLength, ErrorMessage = "{0} must be at least {1} characters.")]
    [MaxLength(ValidationRules.Post.DescriptionMaxLength, ErrorMessage = "{0} must be less than {1} characters.")]
    public string? Description { get; set; }
    
    /// <summary>
    /// New price of the item or service in credits (Kairos).
    /// </summary>
    [Range(ValidationRules.Post.PriceMin, ValidationRules.Post.PriceMax, ErrorMessage = "{0} must be between {1} and {2}.")]
    public int? Price { get; set; }
    
    /// <summary>
    /// New type of the post (should have values 'Offer' or 'Request').
    /// </summary>
    public string? Type { get; set; }
    
    /// <summary>
    /// Identifier of the new post category in which the post is classified.
    /// </summary>
    public long? PostCategoryId { get; set; }
    
    /// <summary>
    /// Determines whether the current object is considered empty.
    /// An object is considered empty if all its properties are <c>null</c>.
    /// </summary>
    /// <returns>
    /// <c>true</c> if all its properties are <c>null</c>; otherwise, <c>false</c>.
    /// </returns>
    public bool IsEmpty()
    {
        return Title == null
               && Description == null
               && Price == null
               && Type == null
               && PostCategoryId == null;
    }
}