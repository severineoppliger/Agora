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
        return await context.Users.FindAsync(id);
    }

    public void AddUser(User user)
    {
        context.Users.Add(user);
    }

    public void UpdateUser(User user)
    {
        context.Entry(user).State = EntityState.Modified;
    }

    public void DeleteUser(User user)
    {
        context.Users.Remove(user);
    }

    public bool UserExists(long id)
    {
        return context.Users.Any(u => u.Id == id);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }
}