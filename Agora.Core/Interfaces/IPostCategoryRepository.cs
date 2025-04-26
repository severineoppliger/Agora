using Agora.Core.Models;

namespace Agora.Core.Interfaces;

public interface IPostCategoryRepository
{
    Task<IReadOnlyList<PostCategory>> GetAllPostCategoriesAsync();
    Task<PostCategory?> GetPostCategoryByIdAsync(long id);
    void AddPostCategory(PostCategory postCategory);
    void DeletePostCategory(PostCategory postCategory);
    Task<bool> SaveChangesAsync();
}