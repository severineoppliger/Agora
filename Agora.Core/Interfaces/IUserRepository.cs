using Agora.Core.Models;

namespace Agora.Core.Interfaces;

public interface IUserRepository
{
    Task<IReadOnlyList<User>> GetAllUsersAsync();
    Task<User?> GetUserByIdAsync(long id);
    void AddUser(User user);
    void UpdateUser(User user);
    void DeleteUser(User user);
    bool UserExists(long id);
    Task<bool> SaveChangesAsync();
}