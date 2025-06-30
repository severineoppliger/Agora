using Agora.API.DTOs.Post;

namespace Agora.API.DTOs.Transaction;

/// <summary>
/// Data Transfer Object used for controller output.  
/// Represents the full details of a <c>Transaction</c>, including linked post, participants, status and timestamps.
/// </summary>
public class TransactionDetailsDto
{
    /// <summary>
    /// Unique identifier of the transaction.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Title of the transaction.
    /// </summary>
    public string Title { get; set; } = String.Empty;
    
    /// <summary>
    /// Price of the transaction, expressed in Kairos credits.
    /// </summary>
    public int Price { get; set; }
    
    /// <summary>
    /// Summary of the published post linked to this transaction, if applicable.
    /// </summary>
    public PostSummaryDto? Post { get; set; }
    
    /// <summary>
    /// Name of the current transaction status (e.g., En attente, Acceptée, Annulée).
    /// </summary>
    public string TransactionStatusName { get; set; } = String.Empty;
    
    /// <summary>
    /// Username of the user who initiated the transaction.
    /// </summary>
    public string InitiatorUsername { get; set; } = String.Empty;
    
    /// <summary>
    /// Username of the buyer involved in the transaction.
    /// </summary>
    public string BuyerUsername { get; set; } = String.Empty;
    
    /// <summary>
    /// Indicates whether the buyer has confirmed the transaction.
    /// </summary>
    public bool BuyerConfirmed { get; set; }
    
    /// <summary>
    /// Username of the seller involved in the transaction.
    /// </summary>
    public string SellerUsername { get; set; } = String.Empty;
    
    /// <summary>
    /// Indicates whether the seller has confirmed the transaction.
    /// </summary>
    public bool SellerConfirmed { get; set; }
    
    /// <summary>
    /// Date and time (UTC) when the transaction was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// Date and time (UTC) when the post was last updated.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
    
    /// <summary>
    /// Optional transaction date provided by users (format "yyyy-MM-dd").
    /// </summary>
    public DateTime? TransactionDate { get; set; }
    
    /// <summary>
    /// Date and time (UTC) when the transaction was completed, if applicable.
    /// </summary>
    public DateTime? CompletedAt { get; set; }
    
    /// <inheritdoc/>
    public override string ToString()
    {
        return $"Id: {Id}, " +
               $"Price: {Price}, " +
               $"Related post id: {(Post is null ? "none" : Post.Id.ToString())}, " +
               $"Transaction status: {TransactionStatusName}, " +
               $"Buyer username: {BuyerUsername}, " +
               $"Buyer has confirmed: {BuyerConfirmed}, "+
               $"Seller username: {SellerUsername}, " +
               $"Seller has confirmed: {SellerConfirmed}, "+
               $"CreatedAt: {CreatedAt:yyyy-MM-dd HH:mm:ss}, " +
               $"TransactionDate : {(TransactionDate.HasValue ? TransactionDate.Value.ToString("yyyy-MM-dd") : "null")}" +
               $"CompletedAt: {(CompletedAt.HasValue ? CompletedAt.Value.ToString("yyyy-MM-dd HH:mm:ss") : "null")}";
    }
}