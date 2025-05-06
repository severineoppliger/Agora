using Agora.Core.Interfaces;

namespace Agora.API.QueryParams;

public class UserQueryParameters : IUserFilter
{
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? SortBy { get; set; }
    public bool SortDesc { get; set; } = false;
}