using Agora.Core.Models;

namespace Agora.Core.Interfaces;

public interface IPostRepository
{
    Task<IReadOnlyList<Post>> GetAllPostsAsync();
    Task<Post?> GetPostByIdAsync(long id);
    void AddPost(Post post);
    void DeletePost(Post post);
    Task<bool> SaveChangesAsync();
}