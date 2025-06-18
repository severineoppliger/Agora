using Agora.Core.Interfaces.Filters;
using Agora.Core.Models;

namespace Agora.Core.Interfaces.Repositories;

public interface IPostCategoryRepository
{
    /// <summary>
    /// Retrieves all post categories, optionally filtered by the provided filter.
    /// </summary>
    /// <param name="filter">An optional filter to apply to the post category query.</param>
    /// <returns>A list of <see cref="PostCategory"/> objects matching the filter.</returns>
    Task<IReadOnlyList<PostCategory>> GetAllPostCategoriesAsync(IPostCategoryFilter filter);
    
    /// <summary>
    /// Retrieves a post category by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the post category.</param>
    /// <returns>
    /// The <see cref="PostCategory"/> if found; otherwise, <c>null</c>.
    /// </returns>
    Task<PostCategory?> GetPostCategoryByIdAsync(long id);
    
    /// <summary>
    /// Adds a new post category to the repository. 
    /// Changes must be saved using <see cref="SaveChangesAsync"/>.
    /// </summary>
    /// <param name="postCategory">The post category to add.</param>
    void AddPostCategory(PostCategory postCategory);
    
    /// <summary>
    /// Deletes an existing post category from the repository.
    /// Changes must be saved using <see cref="SaveChangesAsync"/>.
    /// </summary>
    /// <param name="postCategory">The post category to delete.</param>
    void DeletePostCategory(PostCategory postCategory);
    
    /// <summary>
    /// Persists all pending changes in the repository to the database.
    /// </summary>
    /// <returns>
    /// <c>true</c> if the changes were saved successfully; otherwise, <c>false</c>.
    /// </returns>
    Task<bool> SaveChangesAsync();
    
    /// <summary>
    /// Checks whether a post category with the specified ID exists.
    /// </summary>
    /// <param name="id">The unique identifier of the post category.</param>
    /// <returns>
    /// <c>true</c> if the post category exists; otherwise, <c>false</c>.
    /// </returns>
    Task<bool> PostCategoryExistsAsync(long id);
    
    /// <summary>
    /// Checks whether a post category with the specified name exists.
    /// </summary>
    /// <param name="name">The name of the post category.</param>
    /// <returns>
    /// <c>true</c> if a post category with the given name exists; otherwise, <c>false</c>.
    /// </returns>
    Task<bool> NameExistsAsync(string name);
}