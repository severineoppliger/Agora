using Agora.Core.Interfaces.QueryParameters;

namespace Agora.API.ApiQueryParameters;

/// <summary>
/// Query parameters for filtering and sorting  a list of <c>Transaction</c>
/// </summary>
public class TransactionQueryParameters : ITransactionQueryParameters
{
    /// <summary>
    /// Filter transactions with <c>Price</c> property at least <c>MinPrice</c>.
    /// </summary>
    public int? MinPrice { get; set; }
    
    /// <summary>
    /// Filter transactions with <c>Price</c> property at most <c>MaxPrice</c>.
    /// </summary>
    public int? MaxPrice { get; set; }
    
    /// <summary>
    /// Filter transactions by related post title using substring search.
    /// </summary>
    public string? PostTitle { get; set; }
    
    /// <summary>
    /// Filter transactions by transaction status name using substring search.
    /// </summary>
    public string? TransactionStatusName { get; set; }
    
    /// <summary>
    /// Filter transactions involving a specific username using substring search.
    /// </summary>
    public string? UsersInvolvedUsername { get; set; }
    
    /// <summary>
    /// Property name to sort the results by.
    /// Allowed values: "id", "title", "price", "transactionstatusid" (default), "buyer", "seller" or "createdat".
    /// </summary>
    public string? SortBy { get; set; }
    
    /// <summary>
    /// Indicates whether the sorting should be in descending order. Default is false (ascending).
    /// </summary>
    public bool SortDesc { get; set; }
}