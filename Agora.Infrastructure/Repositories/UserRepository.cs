using System.Security.Claims;
using Agora.Core.Interfaces.Repositories;
using Agora.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Agora.Infrastructure.Repositories;

public class UserRepository(
    UserManager<AppUser> userManager,
    SignInManager<AppUser> signInManager
    ) : IUserRepository
{
    public async Task<IReadOnlyList<AppUser>> GetAllUsersAsync()
    {
        return await userManager.Users.ToListAsync();
    }

    public async Task<AppUser?> GetUserByIdAsync(string id)
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

    public async Task<AppUser?> GetUserByEmailAsync(string email)
    {
        return await userManager.FindByEmailAsync(email);
    }

    public string? GetUserId(ClaimsPrincipal user)
    {
        return userManager.GetUserId(user);   
    }

    public async Task<IdentityResult> AddUserAsync(AppUser user, string password)
    {
        return await signInManager.UserManager.CreateAsync(user, password);
    }

    public async Task<bool> UserExistsAsync(long id)
    {
        throw new NotImplementedException();
    }
}