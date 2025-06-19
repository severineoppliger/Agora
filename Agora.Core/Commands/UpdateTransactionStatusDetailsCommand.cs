namespace Agora.Core.Commands;

/// <summary>
/// Command to update details of an existing <c>TransactionStatus</c>.
/// Only properties with non-null values will be applied.
/// </summary>
public class UpdateTransactionStatusDetailsCommand
{
    /// <summary>
    /// New name of the transaction status (in French).
    /// </summary>
    public string? Name { get; set; }
    
    /// <summary>
    /// New description of the transaction status.
    /// </summary>
    public string? Description { get; set; }
}