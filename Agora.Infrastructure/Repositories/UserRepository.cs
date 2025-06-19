using Agora.Core.Interfaces.QueryParameters;
using Agora.Core.Interfaces.Repositories;
using Agora.Core.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Agora.Infrastructure.Repositories;

public class UserRepository(
    UserManager<User> userManager,
    SignInManager<User> signInManager
    ) : IUserRepository
{
    public async Task<IReadOnlyList<User>> GetAllUsersAsync(IUserQueryParameters queryParameters)
    {
        IQueryable<User> users = userManager.Users.AsQueryable();
        
        if (!string.IsNullOrWhiteSpace(queryParameters.Username))
        {
            users = users.Where(u => u.UserName != null && u.UserName.Contains(queryParameters.Username));
        }
        
        if (!string.IsNullOrWhiteSpace(queryParameters.Email))
        {
            users = users.Where(u => u.Email != null && u.Email.Contains(queryParameters.Email));
        }

        if (queryParameters.MinCredit.HasValue)
        {
            users = users.Where(u => u.Credit >= queryParameters.MinCredit);
        }
        
        if (queryParameters.MaxCredit.HasValue)
        {
            users = users.Where(u => u.Credit <= queryParameters.MaxCredit);
        }
        
        if (queryParameters.MinCreatedAt.HasValue)
        {
            users = users.Where(u => u.CreatedAt >= queryParameters.MinCreatedAt);
        }
        
        if (queryParameters.MaxCreatedAt.HasValue)
        {
            users = users.Where(u => u.CreatedAt <= queryParameters.MaxCreatedAt);
        }
        
        return await ApplySorting(users, queryParameters).ToListAsync();
    }

    public async Task<User?> GetUserByIdAsync(string id)
    {
        return await userManager.Users
            .Include(u=> u.Posts)
                .ThenInclude(p => p.PostCategory)
            .Include(u => u.TransactionsAsBuyer)
                .ThenInclude(t => t.Post)
            .Include(u => u.TransactionsAsBuyer)
                .ThenInclude(t => t.TransactionStatus)
            .Include(u => u.TransactionsAsBuyer)
                .ThenInclude(t => t.Seller)
            .Include(u => u.TransactionsAsSeller)
                .ThenInclude(t => t.Post)
            .Include(u => u.TransactionsAsSeller)
                .ThenInclude(t => t.TransactionStatus)
            .Include(u => u.TransactionsAsSeller)
                .ThenInclude(t => t.Buyer)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await userManager.FindByEmailAsync(email);
    }
    
    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        return await userManager.FindByNameAsync(username);
    }
    
    public async Task<IdentityResult> AddUserAsync(User user, string password)
    {
        return await signInManager.UserManager.CreateAsync(user, password);
    }

    public async Task<IdentityResult> UpdateUserAsync(User user)
    {
        return await userManager.UpdateAsync(user);
    }

    public async Task<bool> UserExistsAsync(string id)
    {
        return await userManager.Users.AnyAsync(u => u.Id == id);
    }
    
        /// <summary>
        /// Applies sorting to the given <see cref="IQueryable{User}"/> based on the specified query parameters.
        /// </summary>
        /// <param name="query">The queryable collection of <see cref="User"/> to sort.</param>
        /// <param name="queryParams">The sorting parameters specifying the property and order (ascending/descending).</param>
        /// <returns>The sorted <see cref="IQueryable{User}"/>.</returns>
    private IQueryable<User> ApplySorting(IQueryable<User> query, IUserQueryParameters queryParams)
    {
        query = queryParams.SortBy?.ToLower() switch
        {
            "id" => queryParams.SortDesc ? query.OrderByDescending(u => u.Id) : query.OrderBy(u => u.Id),
            "username" => queryParams.SortDesc ? query.OrderByDescending(u => u.UserName) : query.OrderBy(u => u.UserName),
            "email" => queryParams.SortDesc ? query.OrderByDescending(u => u.Email) : query.OrderBy(u => u.Email),
            "credit" => queryParams.SortDesc ? query.OrderByDescending(u => u.Credit) : query.OrderBy(u => u.Credit),
            "createdat" => queryParams.SortDesc ? query.OrderByDescending(u => u.CreatedAt) : query.OrderBy(u => u.CreatedAt),
            "lastloginat" => queryParams.SortDesc ? query.OrderByDescending(u => u.LastLoginAt) : query.OrderBy(u => u.LastLoginAt),
            _ => query.OrderBy(p => p.Id)
        };
        return query;
    }

}