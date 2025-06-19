namespace Agora.API.DTOs.TransactionStatus;

/// <summary>
/// Data Transfer Object used for controller output.
/// It represents a summary view of a <c>TransactionStatus</c>, typically used in lists or search results.
/// </summary>
public class TransactionStatusSummaryDto
{
    /// <summary>
    /// Unique identifier of the transaction status.
    /// </summary>
    public long Id { get; set; }
    
    /// <summary>
    /// Name of the transaction status (e.g., En attente, Acceptée, Annulée).
    /// </summary>
    public string Name { get; set; } = String.Empty;
    
    /// <summary>
    /// Description of the transaction status.
    /// </summary>
    public string Description { get; set; } = String.Empty;
    
    /// <summary>
    /// Indicates whether this status is considered final (no further status changes are allowed).
    /// </summary>
    public bool IsFinal { get; set; }
    
    /// <summary>
    /// Indicates whether this status is considered successful from a business perspective.
    /// </summary>
    public bool IsSuccess { get; set; }
}