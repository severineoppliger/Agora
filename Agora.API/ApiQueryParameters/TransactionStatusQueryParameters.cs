using Agora.Core.Interfaces.QueryParameters;

namespace Agora.API.ApiQueryParameters;

public class TransactionStatusQueryParameters : ITransactionStatusQueryParameters
{
    public string? NameOrDescription { get; set; }
    public bool? IsFinal { get; set; }
    public bool? IsSuccess { get; set; }
    public string? SortBy { get; set; }
    public bool SortDesc { get; set; } = false;
}