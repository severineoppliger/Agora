using System.Security.Claims;
using Agora.Core.Models;
using Microsoft.AspNetCore.Identity;

namespace Agora.Core.Interfaces;

public interface IUserRepository
{
    Task<IReadOnlyList<AppUser>> GetAllUsersAsync();
    Task<AppUser?> GetUserByIdAsync(string id);
    Task<AppUser?> GetUserByEmailAsync(string email);
    string? GetUserId(ClaimsPrincipal user);
    Task<IdentityResult> AddUserAsync(AppUser user, string password);
    Task<bool> UserExistsAsync(long id);
}