namespace Agora.API.DTOs.Post;

public class PostSummaryDto
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int Price { get; set; }
    public string TypeName { get; set; } = string.Empty;
    public string StatusName { get; set; } = string.Empty;
    public string PostCategoryName { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
}