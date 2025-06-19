namespace Agora.Core.Commands;

public class UpdatePostDetailsCommand
{
    public string? Title { get; set; }
    
    public string? Description { get; set; }
    
    public int? Price { get; set; }
    
    public string? Type { get; set; }
    
    public long? PostCategoryId { get; set; }
}