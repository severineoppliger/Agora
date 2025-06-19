using Agora.Core.Interfaces.QueryParameters;

namespace Agora.API.ApiQueryParameters;

/// <summary>
/// Query parameters for filtering and sorting a list of <c>TransactionStatus</c>
/// </summary>
public class TransactionStatusQueryParameters : ITransactionStatusQueryParameters
{
    /// <summary>
    /// Filter by name or description of the transaction status using substring search.
    /// </summary>
    public string? NameOrDescription { get; set; }
    
    /// <summary>
    /// Filter by whether the transaction status is final.
    /// </summary>
    public bool? IsFinal { get; set; }
    
    /// <summary>
    /// Filter by whether the transaction status is successful.
    /// </summary>
    public bool? IsSuccess { get; set; }
    
    /// <summary>
    /// Property name to sort the results by.
    /// Allowed values: "id" (default), "name", "isfinal" or "issuccess" (default).
    /// </summary>
    public string? SortBy { get; set; }
    
    /// <summary>
    /// Indicates whether the sorting should be in descending order.
    /// </summary>
    public bool SortDesc { get; set; }
}