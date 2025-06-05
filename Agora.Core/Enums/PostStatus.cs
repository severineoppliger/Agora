namespace Agora.Core.Enums;

public enum PostStatus
{
    Active,                 // Visible in post catalogue publicly, without ongoing transaction
    Inactive,                 // Not visible in post catalogue of other users
    InTransactionActive,    // Visible in post catalogue publicly with ongoing transaction
    InTransactionInactive,    // Not visible in post catalogue of other users, but with ongoing transaction. This stay visible for the two parts of the transactions.
    Deleted                 // For posts that have been deleted but still exist in the database, in case of existing attached transactions
}