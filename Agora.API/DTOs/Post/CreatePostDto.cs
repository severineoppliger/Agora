using System.ComponentModel.DataAnnotations;
using Agora.API.Validation;
using Agora.Core.Validation;

namespace Agora.API.DTOs.Post;

/// <summary>
/// Data Transfer Object for creating a new <c>Post</c>.
/// </summary>
public class CreatePostDto
{
    /// <summary>
    /// Title of the post.
    /// </summary>
    [Required]
    [NotEmptyOrWhitespace]
    [MinLength(ValidationConstants.Post.TitleMinLength, ErrorMessage = "{0} must be at least {1} characters.")]
    [MaxLength(ValidationConstants.Post.TitleMaxLength, ErrorMessage = "{0} must be less than {1} characters.")]
    public string Title { get; set; } = String.Empty;
    
    /// <summary>
    /// Description of the post.
    /// </summary>
    [Required]
    [NotEmptyOrWhitespace]
    [MinLength(ValidationConstants.Post.DescriptionMinLength, ErrorMessage = "{0} must be at least {1} characters.")]
    [MaxLength(ValidationConstants.Post.DescriptionMaxLength, ErrorMessage = "{0} must be less than {1} characters.")]
    public string Description { get; set; } = String.Empty;
    
    /// <summary>
    /// Price of the item or service in credits (Kairos).
    /// </summary>
    [Required]
    [Range(ValidationConstants.Post.PriceMin, ValidationConstants.Post.PriceMax, ErrorMessage = "{0} must be between {1} and {2}.")]
    public int Price { get; set; }
    
    /// <summary>
    /// Type of the post (should have values 'Offer' or 'Request').
    /// </summary>
    [Required]
    public string Type { get; set; } = String.Empty;
    
    /// <summary>
    /// Identifier of the post category in which the post is classified.
    /// </summary>
    [Required]
    public long PostCategoryId { get; set; }
}