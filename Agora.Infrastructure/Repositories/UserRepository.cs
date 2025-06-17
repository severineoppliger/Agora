using Agora.Core.Interfaces.Filters;
using Agora.Core.Interfaces.Repositories;
using Agora.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Agora.Infrastructure.Repositories;

public class UserRepository(
    UserManager<User> userManager,
    SignInManager<User> signInManager
    ) : IUserRepository
{
    public async Task<IReadOnlyList<User>> GetAllUsersAsync(IUserFilter filter)
    {
        IQueryable<User> users = userManager.Users.AsQueryable();
        
        if (!string.IsNullOrWhiteSpace(filter.Username))
        {
            users = users.Where(u => u.UserName != null && u.UserName.Contains(filter.Username));
        }
        
        if (!string.IsNullOrWhiteSpace(filter.Email))
        {
            users = users.Where(u => u.Email != null && u.Email.Contains(filter.Email));
        }

        if (filter.MinCredit.HasValue)
        {
            users = users.Where(u => u.Credit >= filter.MinCredit);
        }
        
        if (filter.MaxCredit.HasValue)
        {
            users = users.Where(u => u.Credit <= filter.MaxCredit);
        }
        
        if (filter.MinCreatedAt.HasValue)
        {
            users = users.Where(u => u.CreatedAt >= filter.MinCreatedAt);
        }
        
        if (filter.MaxCreatedAt.HasValue)
        {
            users = users.Where(u => u.CreatedAt <= filter.MaxCreatedAt);
        }
        
        return await ApplySorting(users, filter).ToListAsync();
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
    
    private IQueryable<User> ApplySorting(IQueryable<User> query, IUserFilter filter)
    {
        query = filter.SortBy?.ToLower() switch
        {
            "id" => filter.SortDesc ? query.OrderByDescending(u => u.Id) : query.OrderBy(u => u.Id),
            "username" => filter.SortDesc ? query.OrderByDescending(u => u.UserName) : query.OrderBy(u => u.UserName),
            "email" => filter.SortDesc ? query.OrderByDescending(u => u.Email) : query.OrderBy(u => u.Email),
            "credit" => filter.SortDesc ? query.OrderByDescending(u => u.Credit) : query.OrderBy(u => u.Credit),
            "createdat" => filter.SortDesc ? query.OrderByDescending(u => u.CreatedAt) : query.OrderBy(u => u.CreatedAt),
            _ => query.OrderBy(p => p.Id)
        };
        return query;
    }

}