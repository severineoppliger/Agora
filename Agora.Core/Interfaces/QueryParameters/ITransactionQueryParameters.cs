namespace Agora.Core.Interfaces.QueryParameters;

/// <summary>
/// Interface that defines query parameters for filtering and sorting a list of <c>Transaction</c>.
/// Implemented by classes used to handle incoming query parameters in API endpoints.
/// </summary>
public interface ITransactionQueryParameters
{
    /// <summary>
    /// Filters transactions with <c>Price</c> property at least <c>MinPrice</c>.
    /// </summary>
    public int? MinPrice { get; set; }
    
    /// <summary>
    /// Filters transactions with <c>Price</c> property at most <c>MaxPrice</c>.
    /// </summary>
    public int? MaxPrice { get; set; }
    
    /// <summary>
    /// Filters transactions by related post title using substring search.
    /// </summary>
    public string? PostTitle { get; set; }
    
    /// <summary>
    /// Filters transactions by transaction status name using substring search.
    /// </summary>
    public string? TransactionStatusName { get; set; }
    
    /// <summary>
    /// Filters transactions involving a specific username using substring search.
    /// </summary>
    public string? UsersInvolvedUsername { get; set; }
    
    /// <summary>
    /// Property name to sort the results by.
    /// Allowed values are given in class <c>SortByOptions.Transaction</c>.
    /// </summary>
    public string? SortBy { get; set; }
    
    /// <summary>
    /// Indicates whether the sorting should be in descending order. Default is false (ascending).
    /// </summary>
    public bool SortDesc { get; set; }
}