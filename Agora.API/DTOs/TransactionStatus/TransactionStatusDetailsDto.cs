using Agora.API.DTOs.Transaction;

namespace Agora.API.DTOs.TransactionStatus;

/// <summary>
/// Data Transfer Object used for controller output.
/// It represents the full details of a <c>TransactionStatus</c>, including the transaction in this status.
/// </summary>
public class TransactionStatusDetailsDto
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
    
    /// <summary>
    /// List of transactions currently associated with this status.
    /// </summary>
    public ICollection<TransactionSummaryDto> Transactions { get; set; } = new List<TransactionSummaryDto>();
    
    /// <inheritdoc/>
    public override string ToString()
    {
        return $"Id: {Id}, Name: {Name}, Description: {Description}, IsFinal: {IsFinal}, IsSuccess: {IsSuccess}, Nb of Transactions: {Transactions.Count}";
    }
}