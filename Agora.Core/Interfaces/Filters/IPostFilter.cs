namespace Agora.Core.Interfaces.Filters;

public interface IPostFilter
{
    public string? TitleOrDescription { get; set; }
    public int? MinPrice { get; set; }
    public int? MaxPrice { get; set; }
    public string? TypeName { get; set; }
    public string? StatusName { get; set; }
    public string? PostCategoryName { get; set; }
    public string? UserName { get; set; }
    public string? SortBy { get; set; }
    public bool SortDesc { get; set; }
}