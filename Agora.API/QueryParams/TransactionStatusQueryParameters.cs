using Agora.Core.Interfaces;

namespace Agora.API.QueryParams;

public class TransactionStatusQueryParameters : ITransactionStatusFilter
{
    public string? NameOrDescription { get; set; }
    public bool? IsFinal { get; set; }
    public bool? IsSuccess { get; set; }
    public string? SortBy { get; set; }
    public bool SortDesc { get; set; } = false;
}