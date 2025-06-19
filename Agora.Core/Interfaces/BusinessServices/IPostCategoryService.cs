using Agora.Core.Common;
using Agora.Core.Interfaces.Filters;
using Agora.Core.Models;

namespace Agora.Core.Interfaces.BusinessServices;

/// <summary>
/// Defines business operations related to post categories,
/// including retrieval and management of post category details.
/// </summary>
public interface IPostCategoryService
{
    /// <summary>
    /// Retrieves all <c>PostCategory</c>, possibly filtered and sorted.
    /// </summary>
    /// <param name="postCategoryFilter">Filter criteria to apply on post categories.</param>
    /// <returns>A successful Result wrapping a list of <c>PostCategory</c>, or failure if an error occurs.</returns>
    public Task<Result<IReadOnlyList<PostCategory>>> GetAllPostCategoriesAsync(IPostCategoryFilter postCategoryFilter);
    
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
    /// Updates name of an existing <c>PostCategory</c> after validating authorization and business rules.
    /// </summary>
    /// <param name="postCategoryId">ID of the <c>PostCategory</c> to update.</param>
    /// <param name="newName">The new name of the <c>PostCategory</c>.</param>
    /// <returns>
    /// Success if update and save are successful,
    /// or failure with appropriate error messages.
    /// </returns>
    public Task<Result> UpdatePostCategoryNameAsync(long postCategoryId, string newName);
    
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