using Agora.Core.Interfaces.Filters;

namespace Agora.API.QueryParams;

public class UserQueryParameters : IUserFilter
{
    public string? Username { get; set; }
    public string? Email { get; set; }
    public int? MinCredit { get; set; }
    public int? MaxCredit { get; set; }
    public DateTime? MinCreatedAt { get; set; }
    public DateTime? MaxCreatedAt { get; set; }
    public string? SortBy { get; set; }
    public bool SortDesc { get; set; } = false;
}