using Agora.Core.Interfaces.Filters;
using Agora.Core.Models;
using Agora.Core.Models.Filters;

namespace Agora.Core.Interfaces.Repositories;

public interface IPostRepository
{
    Task<IReadOnlyList<Post>> GetAllPostsAsync(PostFilter filter);
    Task<Post?> GetPostByIdAsync(long id);
    void AddPost(Post post);
    void DeletePost(Post post);
    Task<bool> SaveChangesAsync();
    Task<bool> PostExistsAsync(long id);
    IQueryable<Post> ApplySorting(IQueryable<Post> query, IPostFilter queryParams);
    Task<bool> IsCategoryInUseAsync(long postCategoryId);

}