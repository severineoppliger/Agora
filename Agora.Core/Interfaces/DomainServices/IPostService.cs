using Agora.Core.Commands;
using Agora.Core.Enums;
using Agora.Core.Models;
using Agora.Core.Models.DomainQueryParameters;
using Agora.Core.Models.Entities;
using Agora.Core.Shared;

namespace Agora.Core.Interfaces.DomainServices;

/// <summary>
/// Defines business logic for managing user posts,
/// including creation, updates, filtering, and status changes.
/// </summary>
public interface IPostService
{
    /// <summary>
    /// Retrieves all <c>Post</c> visible to the user according to authorization rules and visibility mode.
    /// </summary>
    /// <param name="postVisibilityMode">Specifies the scope of posts to retrieve (e.g., catalog only, user-owned, admin view).</param>
    /// <param name="queryParams">Filter criteria to apply when querying posts.</param>
    /// <param name="userContext">Context of the current user making the request.</param>
    /// <returns>A successful Result wrapping a list of posts visible to user, or failure if an error occurs.</returns>
    Task<Result<IReadOnlyList<Post>>> GetAllPostsAsync(
        PostVisibilityMode postVisibilityMode,
        PostQueryParameters queryParams,
        UserContext? userContext
    );
    
    /// <summary>
    /// Retrieves a single <c>Post</c> by its ID if visible to the user.
    /// </summary>
    /// <param name="postId">The ID of the post to retrieve.</param>
    /// <param name="userContext">Context of the current user requesting the post.</param>
    /// <returns>
    /// Success wrapping the <c>Post</c> if found and authorized,
    /// failure with NotFound if missing,
    /// or failure with Unauthorized if the user cannot view it.
    /// </returns>
    Task<Result<Post>> GetPostByIdAsync(
        long postId,
        UserContext? userContext);
    
    /// <summary>
    /// Creates a new <c>Post</c> after validating authorization and business rules.
    /// </summary>
    /// <param name="post">The <c>Post</c> entity to create.</param>
    /// <param name="userContext">Context of the user creating the post.</param>
    /// <returns>
    /// Success wrapping the created <c>Post</c> if successful, or failure with relevant error details.
    /// </returns>
    public Task<Result<Post>> CreatePostAsync(
        Post post,
        UserContext userContext);
    
    /// <summary>
    /// Updates specified details of an existing <c>Post</c> after validating authorization and business rules.
    /// Only non-null values in <paramref name="newDetails"/> are applied.
    /// </summary>
    /// <param name="postId">ID of the <c>Post</c> to update.</param>
    /// <param name="newDetails">The details to update.</param>
    /// <param name="userContext">Context of the user performing the update.</param>
    /// <returns>
    /// Success if update and save are successful,
    /// or failure with appropriate error messages.
    /// </returns>
    public Task<Result> UpdatePostDetailsAsync(
        long postId,
        UpdatePostDetailsCommand newDetails,
        UserContext userContext);
    
    /// <summary>
    /// Changes the status of an existing <c>Post</c> after authorization and validation of state transitions.
    /// </summary>
    /// <param name="postId">ID of the <c>Post</c> to update status for.</param>
    /// <param name="userContext">Context of the user performing the status change.</param>
    /// <param name="targetStatus">The new post status to apply.</param>
    /// <returns>
    /// Success if the status was changed and saved successfully,
    /// or failure with relevant error details if invalid or unauthorized.
    /// </returns>
    public Task<Result> ChangePostStatusAsync(long postId, UserContext userContext, PostStatus targetStatus);
    
    /// <summary>
    /// Delete a <c>Post</c> after validating authorization and business rules.
    /// </summary>
    /// <param name="postId">ID of the <c>Post</c> to delete.</param>
    /// <param name="userContext">Context of the user requesting the deletion.</param>
    /// <returns>
    /// Success if the <c>Post</c> was successfully deleted,
    /// or failure with relevant error details if invalid or unauthorized.
    /// </returns>
    public Task<Result> DeletePostAsync(long postId, UserContext userContext);
}