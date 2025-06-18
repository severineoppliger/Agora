using Agora.Core.Enums;

namespace Agora.Core.Models;

/// <summary>
/// Entity representing a possible status for transactions.
/// Includes descriptive information and semantic meaning (final/success).
/// </summary>
public class TransactionStatus : BaseEntity
{
    /// <summary>
    /// Name of the transaction status.
    /// </summary>
    public required string Name { get; set; }
    
    /// <summary>
    /// Description of the transaction status.
    /// </summary>
    public required string Description { get; set; }
    
    /// <summary>
    /// Indicates whether this status is considered final (no further status changes are allowed).
    /// </summary>
    public required bool IsFinal { get; set; }
    
    /// <summary>
    /// Indicates whether this status is considered successful from a business perspective.
    /// </summary>
    public required bool IsSuccess { get; set; }
    
    /// <summary>
    /// Enum value representing this status.
    /// </summary>
    public required TransactionStatusEnum EnumValue { get; set; }
    
    /// <summary>
    /// Navigation property that represents the collection of <c>Transaction</c> entities linked to this transaction status.
    /// </summary>
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}