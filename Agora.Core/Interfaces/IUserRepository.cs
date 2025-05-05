using Agora.Core.Models;

namespace Agora.Core.Interfaces;

public interface IUserRepository
{
    Task<IReadOnlyList<User>> GetAllUsersAsync(IUserFilter filter);
    Task<User?> GetUserByIdAsync(long id);
    void AddUser(User user);
    void DeleteUser(User user);
    Task<bool> SaveChangesAsync();
    Task<bool> UserExistsAsync(long id);
    Task<bool> UsernameExistsAsync(string username);
    Task<bool> EmailExistsAsync(string email);
}