using Agora.Core.Commands;
using Agora.Core.Interfaces.QueryParameters;
using Agora.Core.Models.Entities;
using Agora.Core.Shared;

namespace Agora.Core.Interfaces.DomainServices;

/// <summary>
/// Defines business operations related to post categories,
/// including retrieval and management of post category details.
/// </summary>
public interface IPostCategoryService
{
    /// <summary>
    /// Retrieves all <c>PostCategory</c>, possibly filtered and sorted.
    /// </summary>
    /// <param name="postCategoryQueryParameters">Filter criteria to apply on post categories.</param>
    /// <returns>A successful Result wrapping a list of <c>PostCategory</c>, or failure if an error occurs.</returns>
    public Task<Result<IReadOnlyList<PostCategory>>> GetAllPostCategoriesAsync(IPostCategoryQueryParameters postCategoryQueryParameters);
    
    /// <summary>
    /// Retrieves a single <c>PostCategory</c> by its ID.
    /// </summary>
    /// <param name="postCategoryId">The ID of the <c>PostCategory</c> to retrieve.</param>
    /// <returns>
    /// Success wrapping the <c>PostCategory</c> if found,
    /// failure with NotFound if missing.
    /// </returns>
    public Task<Result<PostCategory>> GetPostCategoryByIdAsync(long postCategoryId);
    
    /// <summary>
    /// Creates a new <c>PostCategory</c> after validating authorization.
    /// </summary>
    /// <param name="postCategory">The <c>PostCategory</c> entity to create.</param>
    /// <returns>
    /// Success wrapping the created <c>PostCategory</c> if successful,
    /// or failure with relevant error details.
    /// </returns>
    public Task<Result<PostCategory>> CreatePostCategoryAsync(PostCategory postCategory);
    
    /// <summary>
    /// Updates specified details, like the name, of an existing <c>PostCategory</c>
    /// after validating authorization and business rules.
    /// </summary>
    /// <param name="postCategoryId">ID of the <c>PostCategory</c> to update.</param>
    /// <param name="newDetails">The details to update.</param>
    /// <returns>
    /// Success if update and save are successful,
    /// or failure with appropriate error messages.
    /// </returns>
    public Task<Result> UpdatePostCategoryDetailsAsync(long postCategoryId, UpdatePostCategoryDetailsCommand newDetails);
    
    /// <summary>
    /// Delete a <c>PostCategory</c> after validating authorization and business rules.
    /// </summary>
    /// <param name="postCategoryId">ID of the <c>PostCategory</c> to delete.</param>
    /// <returns>
    /// Success if the <c>PostCategory</c> was successfully deleted,
    /// or failure with relevant error details if invalid or unauthorized.
    /// </returns>
    public Task<Result> DeletePostCategoryAsync(long postCategoryId);
}