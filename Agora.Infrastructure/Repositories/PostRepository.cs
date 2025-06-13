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

        if (!string.IsNullOrWhiteSpace(filter.UserName))
        {
            posts = posts.Where(p => p.Owner.UserName!.Contains(filter.UserName));
        }
        
        posts = ApplySorting(posts, filter);
        
        return await posts
            .Include(p => p.Owner)
            .Include(p => p.PostCategory)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Post>> GetAllPostsOfUserAsync(string userId)
    {
        IQueryable<Post> posts = context.Posts;
        posts = posts.Where(p => p.OwnerUserId == userId);
        return await posts
            .Include(p => p.PostCategory)
            .ToListAsync();
    }

    public async Task<Post?> GetPostByIdAsync(long id)
    {
        return await context.Posts
            .Include(p => p.Owner)
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

    public IQueryable<Post> ApplySorting(IQueryable<Post> query, IPostFilter queryParams)
    {
        query = queryParams.SortBy?.ToLower() switch
        {
            "id" => queryParams.SortDesc ? query.OrderByDescending(p => p.Id) : query.OrderBy(p => p.Id),
            "title" => queryParams.SortDesc ? query.OrderByDescending(p => p.Title) : query.OrderBy(p => p.Title),
            "price" => queryParams.SortDesc ? query.OrderByDescending(p => p.Price) : query.OrderBy(p => p.Price),
            "type" => queryParams.SortDesc ? query.OrderByDescending(p => p.Type) : query.OrderBy(p => p.Type),
            "status" => queryParams.SortDesc ? query.OrderByDescending(p => p.Status) : query.OrderBy(p => p.Status),
            "postcategory" => queryParams.SortDesc ? query.OrderByDescending(p => p.PostCategoryId) : query.OrderBy(p => p.PostCategoryId),
            "user" => queryParams.SortDesc ? query.OrderByDescending(p => p.Owner.UserName) : query.OrderBy(p => p.Owner.UserName),
            _ => query.OrderBy(p => p.Id)
        };
        return query;
    }

    public async Task<bool> IsCategoryInUserAsync(long postCategoryId)
    {
        return await context.Posts.AnyAsync(p => p.PostCategoryId == postCategoryId);
    }
}