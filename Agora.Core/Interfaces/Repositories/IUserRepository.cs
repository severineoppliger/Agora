using System.Security.Claims;
using Agora.Core.Models;
using Microsoft.AspNetCore.Identity;

namespace Agora.Core.Interfaces.Repositories;

public interface IUserRepository
{
    Task<IReadOnlyList<AppUser>> GetAllUsersAsync();
    Task<AppUser?> GetUserByIdAsync(string id);
    Task<AppUser?> GetUserByEmailAsync(string email);
    public Task<AppUser?> GetUserByUsernameAsync(string username);
    Task<IdentityResult> AddUserAsync(AppUser user, string password);
    Task<bool> UserExistsAsync(string id);
}