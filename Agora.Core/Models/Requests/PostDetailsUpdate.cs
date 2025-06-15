namespace Agora.Core.Models.Requests;

public class PostDetailsUpdate
{
    public string? Title { get; set; }
    
    public string? Description { get; set; }
    
    public int? Price { get; set; }
    
    public string? Type { get; set; }
    
    public long? PostCategoryId { get; set; }
}