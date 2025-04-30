namespace Agora.API.DTOs.Post;

public abstract class BaseInputPostDto
{
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required int Price { get; set; }
    public required string Type { get; set; }
    public required long PostCategoryId { get; set; }
    
    public void Deconstruct(
        out string title,
        out string description,
        out int price,
        out string type,
        out long postCategoryId)
    {
        title = Title;
        description = Description;
        price = Price;
        type = Type;
        postCategoryId = PostCategoryId;
    }
}