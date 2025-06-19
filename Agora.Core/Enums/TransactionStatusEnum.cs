namespace Agora.Core.Enums;

/// <summary>
/// Specifies the various statuses a transaction can have throughout its lifecycle.
/// </summary>
public enum TransactionStatusEnum
{
    /// <summary>
    /// A transaction proposal has been made and is awaiting the other participant's approval.
    /// </summary>
    Pending = 1,

    /// <summary>
    /// The transaction proposal was cancelled by the initiator before a response.
    /// </summary>
    Cancelled = 2,

    /// <summary>
    /// The transaction proposal was accepted by the other participant.
    /// </summary>
    Accepted = 3,

    /// <summary>
    /// The transaction proposal was refused by the other participant.
    /// </summary>
    Refused = 4,

    /// <summary>
    /// After mutual acceptance, one participant canceled the transaction.
    /// </summary>
    Failed = 5,

    /// <summary>
    /// The transaction was completed and confirmed by one participant.
    /// </summary>
    PartiallyValidated = 6,

    /// <summary>
    /// The transaction was completed and confirmed by both participants.
    /// </summary>
    Completed = 7,

    /// <summary>
    /// The transaction is in dispute following an issue raised by one participant.
    /// </summary>
    InDispute = 8,

    /// <summary>
    /// An administrator reviewed the disputed transaction and accepted it as resolution.
    /// </summary>
    ResolvedAccepted = 9,

    /// <summary>
    /// An administrator reviewed the disputed transaction and canceled it as resolution.
    /// </summary>
    ResolvedCancelled = 10
}
