using System.ComponentModel.DataAnnotations;
using Agora.Core.Validation;

namespace Agora.API.DTOs.Post;

public class UpdatePostDetailsDto
{
    [MinLength(ValidationRules.Post.TitleMinLength, ErrorMessage = "{0} must be at least {1} characters.")]
    [MaxLength(ValidationRules.Post.TitleMaxLength, ErrorMessage = "{0} must be less than {1} characters.")]
    public string? Title { get; set; }
    
    [MinLength(ValidationRules.Post.DescriptionMinLength, ErrorMessage = "{0} must be at least {1} characters.")]
    [MaxLength(ValidationRules.Post.DescriptionMaxLength, ErrorMessage = "{0} must be less than {1} characters.")]
    public string? Description { get; set; }
    
    [Range(ValidationRules.Post.PriceMin, ValidationRules.Post.PriceMax, ErrorMessage = "{0} must be between {1} and {2}.")]
    public int? Price { get; set; }
    
    public string? Type { get; set; }
    
    public long? PostCategoryId { get; set; }
    
    public bool IsEmpty()
    {
        return Title == null
               && Description == null
               && Price == null
               && Type == null
               && PostCategoryId == null;
    }
}