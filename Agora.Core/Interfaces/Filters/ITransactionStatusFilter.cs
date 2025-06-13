namespace Agora.Core.Interfaces.Filters;

public interface ITransactionStatusFilter
{
    public string? NameOrDescription { get; set; }
    public bool? IsFinal { get; set; }
    public bool? IsSuccess { get; set; }
    public string? SortBy { get; set; }
    public bool SortDesc { get; set; }
}