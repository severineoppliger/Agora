namespace Agora.Core.Interfaces;

public interface IPostCategoryFilter
{
    public string? Name { get; set; }
    public string? SortBy { get; set; }
    public bool SortDesc { get; set; }
}