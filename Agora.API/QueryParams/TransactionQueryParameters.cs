using Agora.Core.Interfaces;

namespace Agora.API.QueryParams;

public class TransactionQueryParameters : ITransactionFilter
{
    public int? MinPrice { get; set; }
    public int? MaxPrice { get; set; }
    public string? PostTitle { get; set; }
    public string? TransactionStatusName { get; set; }
    public string? UsersInvolvedUsername { get; set; }
}