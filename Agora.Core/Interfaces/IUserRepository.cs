using Agora.Core.Models;

namespace Agora.Core.Interfaces;

public interface IUserRepository
{
    Task<IReadOnlyList<User>> GetAllUsersAsync();
    Task<User?> GetUserByIdAsync(long id);
    void AddUser(User user);
    void DeleteUser(User user);
    Task<bool> SaveChangesAsync();
    Task<bool> UsernameExistsAsync(string username);
    Task<bool> EmailExistsAsync(string email);
}