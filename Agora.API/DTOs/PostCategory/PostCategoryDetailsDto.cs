using Agora.API.DTOs.Post;

namespace Agora.API.DTOs.PostCategory;

public class PostCategoryDetailsDto
{
    public string Name { get; set; } = String.Empty;
    public List<PostSummaryDto> Posts { get; set; } = new();
}