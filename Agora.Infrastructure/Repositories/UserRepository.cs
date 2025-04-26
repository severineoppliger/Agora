using Agora.Core.Interfaces;
using Agora.Core.Models;
using Agora.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Agora.Infrastructure.Repositories;

public class UserRepository(AgoraDbContext context): IUserRepository
{
    public async Task<IReadOnlyList<User>> GetAllUsersAsync()
    {
        return await context.Users.ToListAsync();
    }

    public async Task<User?> GetUserByIdAsync(long id)
    {
        return await context.Users
            .Include(u => u.Posts)
            .Include(u=> u.TransactionsAsBuyer)
            .Include(u=> u.TransactionsAsSeller)
            .FirstOrDefaultAsync(u=> u.Id == id);
    }

    public void AddUser(User user)
    {
        context.Users.Add(user);
    }
    
    public void DeleteUser(User user)
    {
        context.Users.Remove(user);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> UsernameExistsAsync(string username)
    {
        string normalizedInputUsername = username.Trim().ToLower();
        return await context.Users.AnyAsync(u => u.Username.ToLower().Equals(normalizedInputUsername));
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        string normalizedInputEmail = email.Trim().ToLower();
        return await context.Users.AnyAsync(u => u.Username.ToLower().Equals(normalizedInputEmail));
    }
}