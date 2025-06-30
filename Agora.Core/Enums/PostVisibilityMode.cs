namespace Agora.Core.Enums;

/// <summary>
/// Specifies the visibility mode for retrieving posts.
/// </summary>
public enum PostVisibilityMode
{
    /// <summary>
    /// Only active posts are shown, typically used for public catalog views.
    /// </summary>
    CatalogOnly,
    
    /// <summary>
    /// Shows active and inactive posts belonging to the current user.
    /// </summary>
    CurrentUserPosts,

    /// <summary>
    /// Shows all posts regardless of status, intended for administrators.
    /// </summary>
    AdminView
}