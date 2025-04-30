namespace Agora.API.DTOs.Post;

public class CreatePostDto : BaseInputPostDto
{
    public required long UserId { get; set; }
}