using Agora.Core.Interfaces.Filters;

namespace Agora.API.QueryParams;

public class PostCategoryQueryParameters : IPostCategoryFilter
{
    public string? Name { get; set; }
    public string? SortBy { get; set; }
    public bool SortDesc { get; set; } = false;
}