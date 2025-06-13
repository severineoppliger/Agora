using Agora.Core.Models;

namespace Agora.Core.Interfaces;

public interface IPostRepository
{
    Task<IReadOnlyList<Post>> GetAllPostsAsync(IPostFilter filter);
    Task<IReadOnlyList<Post>> GetAllPostsOfUserAsync(string userId);
    Task<Post?> GetPostByIdAsync(long id);
    void AddPost(Post post);
    void DeletePost(Post post);
    Task<bool> SaveChangesAsync();
    Task<bool> PostExistsAsync(long id);
    IQueryable<Post> ApplySorting(IQueryable<Post> query, IPostFilter queryParams);
    Task<bool> IsCategoryInUserAsync(long postCategoryId);

}