namespace Agora.Core.Enums;

public enum TransactionStatusEnum
{
    Pending = 1,                        // A member has initiated a proposal of transaction, that waits on the approval of the other participant
    Cancelled = 2,                      // The proposal of transaction has been cancelled by the initiator, before any answer of the other participant.
    Accepted = 3,                       // The proposal of transaction has been accepted by the other participant.
    Refused = 4,                        // The proposal of transaction has been refused by the other participant.
    Failed = 5,                         // After acceptance by both parties, one of them finally cancels the transaction.
    PartiallyValidated = 6,             // The transaction was done and confirmed by one party.
    Completed = 7,                      // The transaction was done and confirmed by both parties.
    InDispute = 8,                      // Even if transaction was accepted by both parties, one of them contests about something that didn't occur as expected.
    ResolvedAccepted = 9,               // An admin has reviewed the contention and marked it as finished (i.e. confirmed).
    ResolvedCancelled = 10,             // An admin has reviewed the contention and marked it as cancelled.
}
