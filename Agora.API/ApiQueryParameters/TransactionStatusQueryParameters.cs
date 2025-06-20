using Agora.Core.Interfaces.QueryParameters;

namespace Agora.API.ApiQueryParameters;

/// <summary>
/// Query parameters for filtering and sorting a list of <c>TransactionStatus</c> in API endpoints.
/// </summary>
public class TransactionStatusQueryParameters : ITransactionStatusQueryParameters
{
    /// <inheritdoc />
    public string? NameOrDescription { get; set; }
    
    /// <inheritdoc />
    public bool? IsFinal { get; set; }
    
    /// <inheritdoc />
    public bool? IsSuccess { get; set; }
    
    /// <inheritdoc />
    public string? SortBy { get; set; }
    
    /// <inheritdoc />
    public bool SortDesc { get; set; }
}