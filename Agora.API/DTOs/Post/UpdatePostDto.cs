using System.ComponentModel.DataAnnotations;

namespace Agora.API.DTOs.Post;

public class UpdatePostDto : BaseInputPostDto
{
    [Required]
    public required string Status { get; set; }
}