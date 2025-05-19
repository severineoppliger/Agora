using System.ComponentModel.DataAnnotations;

namespace Agora.API.DTOs.Post;

public class CreatePostDto : BaseInputPostDto
{
    [Required]
    public long UserId { get; set; }
}