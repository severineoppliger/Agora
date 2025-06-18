namespace Agora.API.DTOs.Transaction;

public class TransactionSummaryDto
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
    /// Title of the published post linked to this transaction, if applicable.
    /// </summary>
    public string PostTitle { get; set; } = String.Empty;
    
    /// <summary>
    /// Name of the current transaction status (e.g., En attente, Acceptée, Annulée).
    /// </summary>
    public string TransactionStatusName { get; set; } = String.Empty;
    
    /// <summary>
    /// Username of the buyer involved in the transaction.
    /// </summary>
    public string BuyerUsername { get; set; } = String.Empty;
    
    /// <summary>
    /// Username of the seller involved in the transaction.
    /// </summary>
    public string SellerUsername { get; set; } = String.Empty;
}