using Agora.Core.Models;
using Agora.Core.Models.DomainQueryParameters;
using Agora.Core.Models.Entities;

namespace Agora.Core.Interfaces.Repositories;

public interface IPostRepository
{
    /// <summary>
    /// Retrieves all posts, optionally filtered by the provided queryParameters.
    /// </summary>
    /// <param name="queryParameters">An optional queryParameters to apply to the post query.</param>
    /// <returns>A list of <see cref="Post"/> objects matching the queryParameters.</returns>
    Task<IReadOnlyList<Post>> GetAllPostsAsync(PostQueryParameters queryParameters);
    
    /// <summary>
    /// Retrieves a post by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the post.</param>
    /// <returns>
    /// The <see cref="Post"/> if found; otherwise, <c>null</c>.
    /// </returns>
    Task<Post?> GetPostByIdAsync(long id);
    
    /// <summary>
    /// Adds a new post to the repository. 
    /// Changes must be saved using <see cref="SaveChangesAsync"/>.
    /// </summary>
    /// <param name="post">The post to add.</param>
    void AddPost(Post post);
    
    /// <summary>
    /// Deletes an existing post from the repository.
    /// Changes must be saved using <see cref="SaveChangesAsync"/>.
    /// </summary>
    /// <param name="post">The post to delete.</param>
    void DeletePost(Post post);
    
    /// <summary>
    /// Persists all pending changes in the repository to the database.
    /// </summary>
    /// <returns>
    /// <c>true</c> if the changes were saved successfully; otherwise, <c>false</c>.
    /// </returns>
    Task<bool> SaveChangesAsync();
    
    /// <summary>
    /// Checks whether a post with the specified ID exists.
    /// </summary>
    /// <param name="id">The unique identifier of the post.</param>
    /// <returns>
    /// <c>true</c> if the post exists; otherwise, <c>false</c>.
    /// </returns>
    Task<bool> PostExistsAsync(long id);
    
    /// <summary>
    /// Checks whether a post is classified in a specific <c>PostCategory</c>.
    /// </summary>
    /// <param name="postCategoryId">The unique identifier of the post category.</param>
    /// <returns>
    /// <c>true</c> if any post is categories in the specified <c>PostCategory</c>; otherwise, <c>false</c>.
    /// </returns>
    Task<bool> IsCategoryInUseAsync(long postCategoryId);
}