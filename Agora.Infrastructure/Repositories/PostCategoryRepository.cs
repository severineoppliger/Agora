using Agora.Core.Interfaces.QueryParameters;
using Agora.Core.Interfaces.Repositories;
using Agora.Core.Models.Entities;
using Agora.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Agora.Infrastructure.Repositories;

public class PostCategoryRepository(AgoraDbContext context): IPostCategoryRepository
{
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

    public async Task<PostCategory?> GetPostCategoryByIdAsync(long id)
    {
        return await context.PostCategories
            .Include(pc => pc.Posts)
                .ThenInclude(p => p.Owner)
            .FirstOrDefaultAsync(pc => pc.Id == id);
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
    
    public async Task<bool> PostCategoryExistsAsync(long id)
    {
        return await context.PostCategories.AnyAsync(pc => pc.Id == id);
    }

    public async Task<bool> NameExistsAsync(string name)
    {
        return await context.PostCategories.AnyAsync(pc => pc.Name == name);
    }

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