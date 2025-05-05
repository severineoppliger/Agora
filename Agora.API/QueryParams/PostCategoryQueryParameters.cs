using Agora.Core.Interfaces;

namespace Agora.API.QueryParams;

public class PostCategoryQueryParameters : IPostCategoryFilter
{
    public string? Name { get; set; }
}