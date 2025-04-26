namespace Agora.API.DTOs.Post;

public class UpdatePostDto
{
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required int Price { get; set; }
    public required string Type { get; set; }
    public required string Status { get; set; }
    public required long PostCategoryId { get; set; }
}