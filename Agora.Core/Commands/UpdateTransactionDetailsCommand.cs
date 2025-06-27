namespace Agora.Core.Commands;

/// <summary>
/// Command to update details of an existing <c>Transaction</c>.
/// Only properties with non-null values will be applied.
/// </summary>
public class UpdateTransactionDetailsCommand
{
    /// <summary>
    /// New title of the transaction.
    /// </summary>
    public string? Title { get; set; }
    
    /// <summary>
    /// New price of the transaction in Kairos credits.
    /// </summary>
    public int? Price { get; set; }
    
    /// <summary>
    /// Optional ID of the new related post, if the transaction is linked to a published post.
    /// </summary>
    public long? PostId { get; set; }
    
    /// <summary>
    /// New optional transaction date (format "yyyy-MM-dd"). 
    /// </summary>
    public DateOnly? TransactionDate { get; set; }
}