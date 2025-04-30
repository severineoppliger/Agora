using Agora.Core.Interfaces;
using Agora.Core.Models;
using Agora.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Agora.Infrastructure.Repositories;

public class PostRepository(AgoraDbContext context) : IPostRepository
{
    public async Task<IReadOnlyList<Post>> GetAllPostsAsync()
    {
        return await context.Posts
            .Include(p => p.User)
            .Include(p => p.PostCategory)
            .ToListAsync();
    }

    public async Task<Post?> GetPostByIdAsync(long id)
    {
        return await context.Posts
            .Include(p => p.User)
            .Include(p => p.PostCategory)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public void AddPost(Post post)
    {
        context.Posts.Add(post);
    }

    public void DeletePost(Post post)
    {
        context.Posts.Remove(post);
    }
    
    public async Task<bool> SaveChangesAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> PostExistsAsync(long id)
    {
        return await context.Posts.AnyAsync(p => p.Id == id);
    }
}