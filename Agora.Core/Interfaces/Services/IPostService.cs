using Agora.Core.Interfaces.Filters;
using Agora.Core.Models;

namespace Agora.Core.Interfaces.Services;

public interface IPostService
{
    Task<IReadOnlyList<Post>> GetAllVisiblePostsAsync(
        IPostFilter postFilter,
        bool isAdmin = false,
        string? requestingUserId = null,
        string? targetUserId = null
    );
    
    Task<Post?> GetVisiblePostByIdAsync(
        long postId,
        bool isAdmin, 
        string? requestingUserId);
}