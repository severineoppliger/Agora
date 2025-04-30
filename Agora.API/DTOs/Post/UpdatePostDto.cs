namespace Agora.API.DTOs.Post;

public class UpdatePostDto : BaseInputPostDto
{
    public required string Status { get; set; }
}