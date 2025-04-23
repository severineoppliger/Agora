using Agora.Core.Interfaces;
using Agora.Core.Models;
using Agora.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Agora.Infrastructure.Repositories;

public class PostRepository(AgoraDbContext context) : IPostRepository
{
    public async Task<IReadOnlyList<Post>> GetAllPostsAsync()
    {
        return await context.Posts.ToListAsync();
    }

    public async Task<Post?> GetPostByIdAsync(long id)
    {
        return await context.Posts.FindAsync(id);
    }

    public void AddPost(Post post)
    {
        context.Posts.Add(post);
    }

    public void UpdatePost(Post post)
    {
        context.Entry(post).State = EntityState.Modified;
    }

    public void DeletePost(Post post)
    {
        context.Posts.Remove(post);
    }

    public bool PostExists(long id)
    {
        return context.Posts.Any(p => p.Id == id);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }
}