using Agora.Core.Interfaces.QueryParameters;

namespace Agora.API.ApiQueryParameters;

/// <summary>
/// Query parameters for filtering and sorting a list of <c>Transaction</c> in API endpoints.
/// </summary>
public class TransactionQueryParameters : ITransactionQueryParameters
{
    /// <inheritdoc />
    public int? MinPrice { get; set; }
    
    /// <inheritdoc />
    public int? MaxPrice { get; set; }
    
    /// <inheritdoc />
    public string? PostTitle { get; set; }
    
    /// <inheritdoc />
    public string? TransactionStatusName { get; set; }
    
    /// <inheritdoc />
    public string? UserInvolvedUsername { get; set; }
    
    /// <inheritdoc />
    public string? SortBy { get; set; }
    
    /// <inheritdoc />
    public bool SortDesc { get; set; }
}