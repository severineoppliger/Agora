﻿using System.ComponentModel.DataAnnotations;
using Agora.API.Validation;
using Agora.Core.Validation;

namespace Agora.API.DTOs.PostCategory;

/// <summary>
/// Data Transfer Object for creating a new <c>PostCategory</c>.
/// </summary>
public class CreatePostCategoryDto
{
    /// <summary>
    /// Name of the post category.
    /// </summary>
    [Required]
    [NotEmptyOrWhitespace]
    [MinLength(ValidationConstants.PostCategory.NameMinLength, ErrorMessage = "{0} must be at least {1} characters.")]
    [MaxLength(ValidationConstants.PostCategory.NameMaxLength, ErrorMessage = "{0} must be less than {1} characters.")]
    public string Name { get; set; } = String.Empty;
}