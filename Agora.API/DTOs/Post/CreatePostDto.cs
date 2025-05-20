using System.ComponentModel.DataAnnotations;

namespace Agora.API.DTOs.Post;

public class CreatePostDto : BaseInputPostDto
{
    [Required]
    public string UserId { get; set; } = String.Empty;
}