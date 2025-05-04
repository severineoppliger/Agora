using System.ComponentModel.DataAnnotations;
using Agora.Core.Validation;

namespace Agora.API.DTOs.Post;

public abstract class BaseInputPostDto
{
    [NotEmptyOrWhitespace]
    [MinLength(ValidationRules.Post.TitleMinLength, ErrorMessage = "{0} must be at least {1} characters.")]
    [MaxLength(ValidationRules.Post.TitleMaxLength, ErrorMessage = "{0} must be less than {1} characters.")]
    public required string Title { get; set; }
    
    [NotEmptyOrWhitespace]
    [MinLength(ValidationRules.Post.DescriptionMinLength, ErrorMessage = "{0} must be at least {1} characters.")]
    [MaxLength(ValidationRules.Post.DescriptionMaxLength, ErrorMessage = "{0} must be less than {1} characters.")]
    public required string Description { get; set; }
    
    [Range(ValidationRules.Post.PriceMin, ValidationRules.Post.PriceMax, ErrorMessage = "{0} must be between {1} and {2}.")]
    public required int Price { get; set; }
    public required string Type { get; set; }
    public required long PostCategoryId { get; set; }
}