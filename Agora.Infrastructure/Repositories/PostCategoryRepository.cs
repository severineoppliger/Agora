using Agora.Core.Interfaces;
using Agora.Core.Models;
using Agora.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Agora.Infrastructure.Repositories;

public class PostCategoryRepository(AgoraDbContext context): IPostCategoryRepository
{
    public async Task<IReadOnlyList<PostCategory>> GetAllPostCategoriesAsync()
    {
        return await context.PostCategories.ToListAsync();
    }

    public async Task<PostCategory?> GetPostCategoryByIdAsync(long id)
    {
        return await context.PostCategories.FindAsync(id);
    }

    public void AddPostCategory(PostCategory postCategory)
    {
        context.PostCategories.Add(postCategory);
    }

    public void DeletePostCategory(PostCategory postCategory)
    {
        context.PostCategories.Remove(postCategory);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }
}