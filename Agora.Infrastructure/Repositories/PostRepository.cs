using Agora.Core.Enums;
using Agora.Core.Interfaces.QueryParameters;
using Agora.Core.Interfaces.Repositories;
using Agora.Core.Models.DomainQueryParameters;
using Agora.Core.Models.Entities;
using Agora.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Agora.Infrastructure.Repositories;

/// <summary>
/// Default implementation of <see cref="IPostRepository"/>.
/// </summary>
public class PostRepository(AgoraDbContext context) : IPostRepository
{
    /// <inheritdoc/>
    public async Task<IReadOnlyList<Post>> GetAllPostsAsync(PostQueryParameters queryParameters)
    {
        IQueryable<Post> posts = context.Posts.AsQueryable();

        if (!string.IsNullOrWhiteSpace(queryParameters.TitleOrDescription))
        {
            posts = posts.Where(p => p.Title.Contains(queryParameters.TitleOrDescription) || p.Description.Contains(queryParameters.TitleOrDescription));
        }

        if (queryParameters.MinPrice.HasValue)
        {
            posts = posts.Where(p => p.Price >= queryParameters.MinPrice);
        }

        if (queryParameters.MaxPrice.HasValue)
        {
            posts = posts.Where(p => p.Price <= queryParameters.MaxPrice);
        }

        if (!string.IsNullOrWhiteSpace(queryParameters.TypeName) &&
            Enum.TryParse<PostType>(queryParameters.TypeName, true, out var filterType))
        {
            posts = posts.Where(p => p.Type == filterType);
        }

        if (queryParameters.StatusNames.Any(s => !string.IsNullOrWhiteSpace(s)))
        {
            List<PostStatus> parsedStatuses = queryParameters.StatusNames
                .Select(s => Enum.TryParse<PostStatus>(s, true, out var status) ? (PostStatus?) status : null)
                .Where(e => e.HasValue)
                .Select(e => e!.Value)
                .ToList();

            posts = posts.Where(p => parsedStatuses.Contains(p.Status));
        }
        
        if (!string.IsNullOrWhiteSpace(queryParameters.PostCategoryName))
        {
            posts = posts.Where(p => p.PostCategory!.Name.Contains(queryParameters.PostCategoryName));
        }

        if (!string.IsNullOrWhiteSpace(queryParameters.UserName))
        {
            posts = posts.Where(p => p.Owner!.UserName!.Contains(queryParameters.UserName));
        }
        
        if (!string.IsNullOrWhiteSpace(queryParameters.UserId))
        {
            posts = posts.Where(p => p.OwnerUserId == queryParameters.UserId);
        }
        
        posts = ApplySorting(posts, queryParameters);
        
        return await posts
            .Include(p => p.Owner)
            .Include(p => p.PostCategory)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<Post?> GetPostByIdAsync(long id)
    {
        return await context.Posts
            .Include(p => p.Owner)
            .Include(p => p.PostCategory)
            .Include(p=> p.Transactions)
                .ThenInclude(t => t.Buyer)
            .Include(p=> p.Transactions)
                .ThenInclude(t => t.Seller)
            .Include(p=> p.Transactions)
                .ThenInclude(t => t.TransactionStatus)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    /// <inheritdoc/>
    public void AddPost(Post post)
    {
        context.Posts.Add(post);
    }
    
    /// <inheritdoc/>
    public void DeletePost(Post post)
    {
        context.Posts.Remove(post);
    }
    
    /// <inheritdoc/>
    public async Task<bool> SaveChangesAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }

    /// <inheritdoc/>
    public async Task<bool> PostExistsAsync(long id)
    {
        return await context.Posts.AnyAsync(p => p.Id == id);
    }

    /// <summary>
    /// Applies sorting to the given <see cref="IQueryable{Post}"/> based on the specified query parameters.
    /// </summary>
    /// <param name="query">The queryable collection of <see cref="Post"/> to sort.</param>
    /// <param name="queryParams">The sorting parameters specifying the property and order (ascending/descending).</param>
    /// <returns>The sorted <see cref="IQueryable{Post}"/>.</returns>
    private IQueryable<Post> ApplySorting(IQueryable<Post> query, IPostQueryParameters queryParams)
    {
        query = queryParams.SortBy?.ToLower() switch
        {
            "id" => queryParams.SortDesc ? query.OrderByDescending(p => p.Id) : query.OrderBy(p => p.Id),
            "title" => queryParams.SortDesc ? query.OrderByDescending(p => p.Title) : query.OrderBy(p => p.Title),
            "price" => queryParams.SortDesc ? query.OrderByDescending(p => p.Price) : query.OrderBy(p => p.Price),
            "type" => queryParams.SortDesc ? query.OrderByDescending(p => p.Type) : query.OrderBy(p => p.Type),
            "status" => queryParams.SortDesc ? query.OrderByDescending(p => p.Status) : query.OrderBy(p => p.Status),
            "postcategoryid" => queryParams.SortDesc ? query.OrderByDescending(p => p.PostCategoryId) : query.OrderBy(p => p.PostCategoryId),
            "postcategoryname" => queryParams.SortDesc ? query.OrderByDescending(p => p.PostCategory!.Name) : query.OrderBy(p => p.PostCategory!.Name),
            "user" => queryParams.SortDesc ? query.OrderByDescending(p => p.Owner!.UserName) : query.OrderBy(p => p.Owner!.UserName),
            "createdat" => queryParams.SortDesc ? query.OrderByDescending(p => p.CreatedAt) : query.OrderBy(p => p.CreatedAt),
            "updatedat" => queryParams.SortDesc ? query.OrderByDescending(p => p.UpdatedAt) : query.OrderBy(p => p.UpdatedAt),
            _ => query.OrderByDescending(p => p.CreatedAt)
        };
        return query;
    }

    /// <inheritdoc/>
    public async Task<bool> IsCategoryInUseAsync(long postCategoryId)
    {
        return await context.Posts.AnyAsync(p => p.PostCategoryId == postCategoryId);
    }
}