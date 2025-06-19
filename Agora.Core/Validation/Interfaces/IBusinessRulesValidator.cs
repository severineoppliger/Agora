using Agora.Core.Commands;
using Agora.Core.Enums;
using Agora.Core.Models;
using Agora.Core.Models.Entities;
using Agora.Core.Shared;

namespace Agora.Core.Validation.Interfaces;

/// <summary>
/// Interface defining methods to validate business rules on domain entities.
/// </summary>
public interface IBusinessRulesValidator
{
    /// <summary>
    /// Validates the creation of a new post category by checking business rules,
    /// such as ensuring the category name does not already exist.
    /// </summary>
    /// <param name="postCategory">The post category entity to validate.</param>
    /// <returns>
    /// A <see cref="Result"/> indicating whether the post category passes validation.
    /// Returns a failure result if the name is already taken or violates other business rules.
    /// </returns>
    public Task<Result> ValidateNewPostCategoryAsync(PostCategory postCategory);
    
    /// <summary>
    /// Validates the update of an existing post category by verifying business rules,
    /// such as ensuring the new name is different from the current one and does not already exist.
    /// </summary>
    /// <param name="postCategory">The existing post category entity.</param>
    /// <param name="newDetails">The new data proposed for the post category.</param>
    /// <returns>
    /// A <see cref="Result"/> indicating whether the update is valid.
    /// Returns a failure result if the new name is null, identical to the current name or already exists.
    /// </returns>
    public Task<Result> ValidatePostCategoryUpdateAsync(
        PostCategory postCategory, 
        UpdatePostCategoryDetailsCommand newDetails);
    
    /// <summary>
    /// Validates the deletion of an existing post category by verifying it is not used by any post.
    /// </summary>
    /// <param name="postCategory">The post category to delete.</param>
    /// <returns>
    /// A <see cref="Result"/> indicating whether the deletion is valid.
    /// Returns a failure result if the category is used by one or several posts.
    /// </returns>
    public Task<Result> ValidatePostCategoryDeletionAsync(PostCategory postCategory);

    /// <summary>
    /// Validates whether a new post can be created by the current user based on business rules.
    /// </summary>
    /// <param name="newPost">The new post to validate.</param>
    /// <param name="userContext">The context of the user attempting to create the post.</param>
    /// <returns>
    /// A <see cref="Result"/> indicating success if the post is valid, or failure if a rule is violated (e.g., duplicate title).
    /// </returns>
    /// <remarks>
    /// The validation checks that the user does not already have another post (active or inactive)
    /// with the same title as the new post.
    /// </remarks>
    public Task<Result> ValidateNewPostAsync(Post newPost, UserContext userContext);
    
    /// <summary>
    /// Validates the update of an existing post by verifying business rules,
    /// such as ensuring the new title is not duplicate with a title of another post of the user.
    /// </summary>
    /// <param name="oldPost">The existing post entity.</param>
    /// <param name="newDetails">The new data proposed for the post.</param>
    /// <param name="userContext">The context of the user attempting to update the post.</param>
    /// <returns>
    /// A <see cref="Result"/> indicating whether the update is valid.
    /// Returns a failure result if the new name is identical to the current name or already exists.
    /// </returns>
    public Task<Result> ValidatePostUpdateAsync(Post oldPost, UpdatePostDetailsCommand newDetails, UserContext userContext);
    
    /// <summary>
    /// Validates whether a post is allowed to transition from its current status to a new specified status,
    /// based on business rules and the current state of the post.
    /// </summary>
    /// <param name="oldPost">The existing post with its current status and related data.</param>
    /// <param name="newStatus">The new status to which the post is intended to transition.</param>
    /// <returns>
    /// A <see cref="Result"/> indicating whether the status change is valid, including an error message if invalid.
    /// </returns>
    public Result ValidatePostStatusChangeAsync(Post oldPost, PostStatus newStatus);

    /// <summary>
    /// Validates whether a post can be deleted by a specific user, taking into account business rules
    /// such as ownership, status, and active transactions.
    /// </summary>
    /// <param name="post">The post that is requested to be deleted.</param>
    /// <param name="userContext">The context of the user attempting to delete the post.</param>
    /// <returns>
    /// A <see cref="Task{Result}"/> representing the asynchronous validation result. The result indicates
    /// whether deletion is permitted and includes an error message if it is not.
    /// </returns>
    public Task<Result> ValidatePostDeletionAsync(Post post, UserContext userContext);

    /// <summary>
    /// Validates the update of an existing transaction status by verifying business rules,
    /// such as ensuring the new name is not duplicate with a name of another transaction status.
    /// </summary>
    /// <param name="oldTransactionStatus">The existing transaction status entity.</param>
    /// <param name="newDetails">The new data proposed for the transaction status.</param>
    /// <returns>
    /// A <see cref="Result"/> indicating whether the update is valid.
    /// Returns a failure result if the new name is identical to the current name or already exists.
    /// </returns>
    public Task<Result> ValidateTransactionStatusUpdateAsync(TransactionStatus oldTransactionStatus,
        UpdateTransactionStatusDetailsCommand newDetails);

    /// <summary>
    /// Validates whether a user is authorized to change a transaction's status from the specified old status to the new status,
    /// based on business rules and the current user's context.
    /// </summary>
    /// <param name="transaction">The transaction being updated.</param>
    /// <param name="oldStatus">The current status of the transaction.</param>
    /// <param name="newStatus">The desired new status for the transaction.</param>
    /// <param name="userContext">Information about the current user's identity and role.</param>
    /// <returns>A failure result if the transition is not permitted; otherwise, a success result.</returns>
    public Result ValidateTransactionStatusChange(
        Transaction transaction,
        TransactionStatusEnum oldStatus,
        TransactionStatusEnum newStatus,
        UserContext userContext);

    /// <summary>
    /// Validates the internal consistency and business rules of a transaction (e.g., buyer/seller identity, ownership, credit).
    /// </summary>
    /// <param name="transaction">The transaction to validate.</param>
    /// <returns>A result indicating success if the transaction is valid; otherwise, a result containing the relevant validation errors.</returns>
    public Result ValidateNewTransaction(Transaction transaction);

    /// <summary>
    /// Validates the update of an existing transaction by verifying business rules.
    /// </summary>
    /// <param name="transaction">The existing transaction entity.</param>
    /// <returns>
    /// A <see cref="Result"/> indicating whether the update is valid.
    /// Returns a failure result if any business rule is not respected.
    /// </returns>
    public Result ValidateTransactionUpdate(Transaction transaction);
    
    /// <summary>
    /// Validates whether a provided <c>SortBy</c> value is allowed for the given entity type,
    /// and returns a <see cref="Result"/> indicating success or failure.
    /// </summary>
    /// <param name="sortByValue">The <c>SortBy</c> value to validate. Can be <c>null</c> or empty.</param>
    /// <param name="allowedValues">The set of allowed <c>SortBy</c> values for the entity.</param>
    /// <returns>
    /// A <see cref="Result"/> that is successful if <paramref name="sortByValue"/> is valid or empty;
    /// otherwise, a failed <see cref="Result"/> containing an appropriate error message.
    /// </returns>
    public Result ValidateSortBy(string? sortByValue, HashSet<string> allowedValues);
}