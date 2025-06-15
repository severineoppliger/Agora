using Agora.Core.Common;
using Agora.Core.Enums;
using Agora.Core.Interfaces.Filters;
using Agora.Core.Models;
using Agora.Core.Models.Requests;

namespace Agora.Core.Interfaces.BusinessServices;

public interface IPostCategoryService
{
    /// <summary>
    /// Retrieves all post categories, possibly filtered and sorted.
    /// </summary>
    /// <param name="postCategoryFilter">Filter criteria to apply on post categories.</param>
    /// <returns>A successful Result wrapping a list of post categories, or failure if an error occurs.</returns>
    public Task<Result<IReadOnlyList<PostCategory>>> GetAllPostCategoriesAsync(IPostCategoryFilter postCategoryFilter);
    
    /// <summary>
    /// Retrieves a single post category by its ID.
    /// </summary>
    /// <param name="postCategoryId">The ID of the post category to retrieve.</param>
    /// <returns>
    /// Success wrapping the post category if found,
    /// failure with NotFound if missing.
    /// </returns>
    public Task<Result<PostCategory>> GetPostCategoryByIdAsync(long postCategoryId);
    
    /// <summary>
    /// Creates a new post category after validating authorization.
    /// </summary>
    /// <param name="postCategory">The post category entity to create.</param>
    /// <returns>
    /// Success wrapping the created post category if successful,
    /// or failure with relevant error details.
    /// </returns>
    public Task<Result<PostCategory>> CreatePostCategoryAsync(PostCategory postCategory);
    
    /// <summary>
    /// Updates name of an existing post category after authorization.
    /// </summary>
    /// <param name="postCategoryId">ID of the post category to update.</param>
    /// <param name="newName">The new name of the post category.</param>
    /// <returns>
    /// Success if update and save are successful,
    /// or failure with appropriate error messages.
    /// </returns>
    public Task<Result> UpdatePostCategoryNameAsync(long postCategoryId, string newName);
    
    /// <summary>
    /// Delete a post category after validating authorization and business rules.
    /// </summary>
    /// <param name="postCategoryId">ID of the post category to delete.</param>
    /// <returns>
    /// Success if the post category was successfully deleted,
    /// or failure with relevant error details if invalid or unauthorized.
    /// </returns>
    public Task<Result> DeletePostCategoryAsync(long postCategoryId);
}