namespace Agora.Core.Interfaces.QueryParameters;

/// <summary>
/// Interface that defines query parameters for filtering and sorting a list of <c>TransactionStatus</c>.
/// Implemented by classes used to handle incoming query parameters in API endpoints.
/// </summary>
public interface ITransactionStatusQueryParameters
{
    /// <summary>
    /// Filters by name or description of the transaction status using substring search.
    /// </summary>
    public string? NameOrDescription { get; set; }
    
    /// <summary>
    /// Filters by whether the transaction status is final.
    /// </summary>
    public bool? IsFinal { get; set; }
    
    /// <summary>
    /// Filters by whether the transaction status is successful.
    /// </summary>
    public bool? IsSuccess { get; set; }
    
    /// <summary>
    /// Property name to sort the results by.
    /// Allowed values are given in class <c>SortByOptions.TransactionStatus</c>.
    /// </summary>
    public string? SortBy { get; set; }
    
    /// <summary>
    /// Indicates whether the sorting should be in descending order.
    /// </summary>
    public bool SortDesc { get; set; }
}