using Agora.Core.Enums;
using Agora.Core.Interfaces;
using Agora.Core.Models;
using Agora.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Agora.Infrastructure.Repositories;

public class PostRepository(AgoraDbContext context) : IPostRepository
{
    public async Task<IReadOnlyList<Post>> GetAllPostsAsync(IPostFilter filter)
    {
        IQueryable<Post> posts = context.Posts.AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.TitleOrDescription))
        {
            posts = posts.Where(p => p.Title.Contains(filter.TitleOrDescription) || p.Description.Contains(filter.TitleOrDescription));
        }

        if (filter.MinPrice.HasValue)
        {
            posts = posts.Where(p => p.Price >= filter.MinPrice);
        }

        if (filter.MaxPrice.HasValue)
        {
            posts = posts.Where(p => p.Price <= filter.MaxPrice);
        }

        if (!string.IsNullOrWhiteSpace(filter.TypeName) &&
            Enum.TryParse<PostType>(filter.TypeName, true, out var filterType))
        {
            posts = posts.Where(p => p.Type == filterType);
        }
        
        if (!string.IsNullOrWhiteSpace(filter.StatusName) &&
            Enum.TryParse<PostStatus>(filter.StatusName, true, out var filterStatus))
        {
            posts = posts.Where(p => p.Status == filterStatus);
        }
        
        if (!string.IsNullOrWhiteSpace(filter.PostCategoryName))
        {
            posts = posts.Where(p => p.PostCategory.Name.Contains(filter.PostCategoryName));
        }

        if (!string.IsNullOrWhiteSpace(filter.Username))
        {
            posts = posts.Where(p => p.User.Username.Contains(filter.Username));
        }
        
        return await posts
            .Include(p => p.User)
            .Include(p => p.PostCategory)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Post>> GetAllPostsOfUserAsync(long userId)
    {
        IQueryable<Post> posts = context.Posts;
        posts = posts.Where(p => p.UserId == userId);
        return await posts
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