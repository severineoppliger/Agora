using Agora.Core.Interfaces.QueryParameters;

namespace Agora.Core.Models.DomainQueryParameters;

/// <summary>
/// Query parameters for filtering and sorting posts a list of <c>Transaction</c> from the domain layer.
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
    public string? UsersInvolvedUsername { get; set; }
    
    /// <summary>
    /// Filters transactions where the given user id matches buyer or seller <c>User</c> unique identifier.
    /// </summary>
    public string? UserId { get; set; }
    
    /// <inheritdoc />
    public string? SortBy { get; set; }
    
    /// <inheritdoc />
    public bool SortDesc { get; set; }
}