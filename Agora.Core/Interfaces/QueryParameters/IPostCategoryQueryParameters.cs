namespace Agora.Core.Interfaces.QueryParameters;

public interface IPostCategoryQueryParameters
{
    public string? Name { get; set; }
    public string? SortBy { get; set; }
    public bool SortDesc { get; set; }
}