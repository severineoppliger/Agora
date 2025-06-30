namespace Agora.Core.Enums;

/// <summary>
/// Specifies the possible statuses of a post.
/// </summary>
public enum PostStatus
{
    /// <summary>
    /// The post is publicly visible in the catalog.
    /// </summary>
    Active,

    /// <summary>
    /// The post is hidden from other users unless they are involved in an ongoing transaction related to it.
    /// </summary>
    Inactive,

    /// <summary>
    /// The post has been deleted but remains in the database due to existing related transactions.
    /// </summary>
    Deleted
}