using Agora.Core.Interfaces.QueryParameters;

namespace Agora.Core.Models.DomainQueryParameters;

public class PostQueryParameters : IPostQueryParameters
{
    public string? TitleOrDescription { get; set; }
    public int? MinPrice { get; set; }
    public int? MaxPrice { get; set; }
    public string? TypeName { get; set; }
    public List<string> StatusNames { get; set; } = [];
    public string? PostCategoryName { get; set; }
    public string? UserName { get; set; }
    public string? UserId { get; set; }
    public string? SortBy { get; set; }
    public bool SortDesc { get; set; }
}