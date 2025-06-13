namespace Agora.Core.Interfaces.Filters;

public interface IUserFilter
{
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? SortBy { get; set; }
    public bool SortDesc { get; set; }
    
}