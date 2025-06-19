using Agora.Core.Interfaces.QueryParameters;

namespace Agora.API.ApiQueryParameters;

public class TransactionQueryParameters : ITransactionQueryParameters
{
    public int? MinPrice { get; set; }
    public int? MaxPrice { get; set; }
    public string? PostTitle { get; set; }
    public string? TransactionStatusName { get; set; }
    public string? UsersInvolvedUsername { get; set; }
    public string? SortBy { get; set; }
    public bool SortDesc { get; set; } = false;
}