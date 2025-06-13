namespace Agora.Core.Interfaces.Filters;

public interface IPostCategoryFilter
{
    public string? Name { get; set; }
    public string? SortBy { get; set; }
    public bool SortDesc { get; set; }
}