namespace Agora.Core.Enums;

public enum TransactionStatusEnum
{
    Waiting,                        // A member has initiated a proposal of transaction, that waits on the approval of the other participant
    Cancelled,                      // The proposal of transaction has been cancelled by the initiator, before any answer of the other participant.
    Accepted,                       // The proposal of transaction has been accepted by the other participant.
    Refused,                        // The proposal of transaction has been refused by the other participant.
    Failed,                         // After acceptance by both parties, one of them finally cancels the transaction.
    PartiallyValidated,             // The transaction was done and confirmed by one party.
    Finished,                       // The transaction was done and confirmed by both parties.
    Contested,                      // Even if transaction was accepted by both parties, one of them contests about something that didn't occur as expected.
    ReviewedByAdminAndConfirmed,    // An admin has reviewed the contention and marked it as finished (i.e. confirmed).
    ReviewedByAdminAndCancelled,    // An admin has reviewed the contention and marked it as cancelled.
}
