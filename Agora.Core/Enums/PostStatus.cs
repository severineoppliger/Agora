namespace Agora.Core.Enums;

public enum PostStatus
{
    Draft,              // Not active yet, so only visible for the current user
    Active,             // Visible in post catalogue publicly
    InTransaction,      // Visible in post catalogue publicly with ongoing transaction
    Closed              // Not visible in post catalogue of other users
}