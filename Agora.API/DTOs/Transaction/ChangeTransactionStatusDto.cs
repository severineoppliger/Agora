using System.ComponentModel.DataAnnotations;
using Agora.Core.Enums;

namespace Agora.API.DTOs.Transaction;

/// <summary>
/// Data Transfer Object used to request a change of status for a specific <c>Transaction</c>.
/// </summary>
public class ChangeTransactionStatusDto
{
    /// <summary>
    /// New status to assign to the transaction.
    /// Must be a valid value from <see cref="TransactionStatusEnum"/>.
    /// </summary>
    [Required]
    public TransactionStatusEnum Status { get; set; }
}