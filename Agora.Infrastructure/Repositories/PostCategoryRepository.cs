using Agora.Core.Interfaces.QueryParameters;
using Agora.Core.Interfaces.Repositories;
using Agora.Core.Models.Entities;
using Agora.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Agora.Infrastructure.Repositories;

/// <inheritdoc/>
public class PostCategoryRepository(AgoraDbContext context): IPostCategoryRepository
{
    /// <inheritdoc/>
    public async Task<IReadOnlyList<PostCategory>> GetAllPostCategoriesAsync(IPostCategoryQueryParameters queryParameters)
    {
        IQueryable<PostCategory> postCategories = context.PostCategories.AsQueryable();

        if (!string.IsNullOrWhiteSpace(queryParameters.Name))
        {
            postCategories = postCategories.Where(pc => pc.Name.Contains(queryParameters.Name));
        }
        
        postCategories = ApplySorting(postCategories, queryParameters);
        
        return await postCategories.ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<PostCategory?> GetPostCategoryByIdAsync(long id)
    {
        return await context.PostCategories
            .Include(pc => pc.Posts)
                .ThenInclude(p => p.Owner)
            .FirstOrDefaultAsync(pc => pc.Id == id);
    }

    /// <inheritdoc/>
    public void AddPostCategory(PostCategory postCategory)
    {
        context.PostCategories.Add(postCategory);
    }

    /// <inheritdoc/>
    public void DeletePostCategory(PostCategory postCategory)
    {
        context.PostCategories.Remove(postCategory);
    }

    /// <inheritdoc/>
    public async Task<bool> SaveChangesAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }
    
    /// <inheritdoc/>
    public async Task<bool> PostCategoryExistsAsync(long id)
    {
        return await context.PostCategories.AnyAsync(pc => pc.Id == id);
    }

    /// <inheritdoc/>
    public async Task<bool> NameExistsAsync(string name)
    {
        return await context.PostCategories.AnyAsync(pc => pc.Name == name);
    }

    /// <summary>
    /// Applies sorting to the given <see cref="IQueryable{PostCategory}"/> based on the specified query parameters.
    /// </summary>
    /// <param name="query">The queryable collection of <see cref="PostCategory"/> to sort.</param>
    /// <param name="queryParams">The sorting parameters specifying the property and order (ascending/descending).</param>
    /// <returns>The sorted <see cref="IQueryable{PostCategory}"/>.</returns>
    private IQueryable<PostCategory> ApplySorting(IQueryable<PostCategory> query, IPostCategoryQueryParameters queryParams)
    {
        query = queryParams.SortBy?.ToLower() switch
        {
            "id" => queryParams.SortDesc ? query.OrderByDescending(pc => pc.Id) : query.OrderBy(pc => pc.Id),
            "name" => queryParams.SortDesc ? query.OrderByDescending(pc => pc.Name) : query.OrderBy(pc => pc.Name), 
            _ => query.OrderBy(p => p.Id)
        };
        return query;
    }
}