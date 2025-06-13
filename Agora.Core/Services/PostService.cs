using Agora.Core.BusinessRules.Interfaces;
using Agora.Core.Enums;
using Agora.Core.Interfaces;
using Agora.Core.Interfaces.Filters;
using Agora.Core.Interfaces.Repositories;
using Agora.Core.Interfaces.Services;
using Agora.Core.Models;

namespace Agora.Core.Services;

public class PostService(IPostRepository repo, IVisibilityBusinessRules visibilityBusinessRules) : IPostService
{
    public async Task<IReadOnlyList<Post>> GetAllVisiblePostsAsync(
        IPostFilter postFilter,
        bool isAdmin = false,
        string? requestingUserId = null,
        string? targetUserId = null
        )
    {
        // Apply the postFilter from the user Request
        IReadOnlyList<Post> posts = await repo.GetAllPostsAsync(postFilter);

        if (!string.IsNullOrWhiteSpace(targetUserId))
        {
            posts = posts.Where(p => p.OwnerUserId == targetUserId).ToList();
        }

        return posts
            .Where(post =>
            {
                bool isOwner = post.OwnerUserId == requestingUserId;
                return visibilityBusinessRules.CanViewPost(post.Status, isAdmin, isOwner);
            })
            .ToList();;
    }

    public async Task<Post?> GetVisiblePostByIdAsync(long postId, bool isAdmin, string? requestingUserId)
    {
        Post? post = await repo.GetPostByIdAsync(postId);
        if (post is null)
        {
            return null;
        }

        return visibilityBusinessRules.CanViewPost(post.Status, isAdmin, post.OwnerUserId == requestingUserId)
            ? post
            : null;
    }
}
    
   