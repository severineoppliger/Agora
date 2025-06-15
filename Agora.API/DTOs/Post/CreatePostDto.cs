using System.ComponentModel.DataAnnotations;
using Agora.Core.Validation;

namespace Agora.API.DTOs.Post;

public class CreatePostDto
{
    [Required]
    [NotEmptyOrWhitespace]
    [MinLength(ValidationRules.Post.TitleMinLength, ErrorMessage = "{0} must be at least {1} characters.")]
    [MaxLength(ValidationRules.Post.TitleMaxLength, ErrorMessage = "{0} must be less than {1} characters.")]
    public string Title { get; set; } = String.Empty;
    
    [Required]
    [NotEmptyOrWhitespace]
    [MinLength(ValidationRules.Post.DescriptionMinLength, ErrorMessage = "{0} must be at least {1} characters.")]
    [MaxLength(ValidationRules.Post.DescriptionMaxLength, ErrorMessage = "{0} must be less than {1} characters.")]
    public string Description { get; set; } = String.Empty;
    
    [Required]
    [Range(ValidationRules.Post.PriceMin, ValidationRules.Post.PriceMax, ErrorMessage = "{0} must be between {1} and {2}.")]
    public int Price { get; set; }
    
    [Required]
    public string Type { get; set; } = String.Empty;
    
    [Required]
    public long PostCategoryId { get; set; }
}