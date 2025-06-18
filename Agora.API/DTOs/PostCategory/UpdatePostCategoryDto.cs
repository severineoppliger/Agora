using System.ComponentModel.DataAnnotations;
using Agora.Core.Validation;

namespace Agora.API.DTOs.PostCategory;

/// <summary>
/// Data Transfer Object for updating an existing <c>PostCategory</c>.
/// </summary>
public class UpdatePostCategoryDto
{
    /// <summary>
    /// New name for the post category.
    /// </summary>
    [Required]
    [NotEmptyOrWhitespace]
    [MinLength(ValidationRules.PostCategory.NameMinLength, ErrorMessage = "{0} must be at least {1} characters.")]
    [MaxLength(ValidationRules.PostCategory.NameMaxLength, ErrorMessage = "{0} must be less than {1} characters.")]
    public string Name { get; set; } = String.Empty;
}