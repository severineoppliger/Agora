namespace Agora.Core.Enums;

public enum PostStatus
{
    Active,                     // Visible in post catalogue publicly
    Inactive,                   // Not visible by other users except if user is involved in an ongoing transaction linked to the post
    Deleted                     // For posts that have been deleted but still exist in the database, in case of existing attached transactions
}