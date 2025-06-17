using System.Security.Claims;
using Agora.Core.Interfaces.Filters;
using Agora.Core.Models;
using Microsoft.AspNetCore.Identity;

namespace Agora.Core.Interfaces.Repositories;

public interface IUserRepository
{
    //TODO Description tous les repository
    //TODO implémenter le filtrage/sorting
    Task<IReadOnlyList<User>> GetAllUsersAsync(IUserFilter filter);
    Task<User?> GetUserByIdAsync(string id);
    Task<User?> GetUserByEmailAsync(string email);
    public Task<User?> GetUserByUsernameAsync(string username);
    Task<IdentityResult> AddUserAsync(User user, string password);
    Task<IdentityResult> UpdateUserAsync(User user);
    Task<bool> UserExistsAsync(string id);
}