using Agora.Core.Interfaces.QueryParameters;

namespace Agora.API.ApiQueryParameters;

public class PostCategoryQueryParameters : IPostCategoryQueryParameters
{
    public string? Name { get; set; }
    public string? SortBy { get; set; }
    public bool SortDesc { get; set; } = false;
}