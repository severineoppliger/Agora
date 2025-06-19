namespace Agora.Core.Interfaces.QueryParameters;

public interface IUserQueryParameters
{
    public string? Username { get; set; }
    public string? Email { get; set; }
    public int? MinCredit { get; set; }
    public int? MaxCredit { get; set; }
    public DateTime? MinCreatedAt { get; set; }
    public DateTime? MaxCreatedAt { get; set; }
    public string? SortBy { get; set; }
    public bool SortDesc { get; set; }
    
}