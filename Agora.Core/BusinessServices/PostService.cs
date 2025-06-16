using Agora.Core.BusinessRules.Interfaces;
using Agora.Core.Common;
using Agora.Core.Enums;
using Agora.Core.Interfaces.BusinessServices;
using Agora.Core.Interfaces.Repositories;
using Agora.Core.Models;
using Agora.Core.Models.Filters;
using Agora.Core.Models.Requests;

namespace Agora.Core.BusinessServices;

/// <inheritdoc />
public class PostService(
    IPostRepository postRepo, 
    ITransactionRepository transactionRepo,
    IAuthorizationBusinessRules authorizationBusinessRules,
    IBusinessRulesValidator businessRulesValidator) : IPostService
{
    private const string EntityName = "post";

    /// <inheritdoc />
    public async Task<Result<IReadOnlyList<Post>>> GetAllPostsAsync(
        PostVisibilityMode postVisibilityMode,
        PostFilter postFilter,
        UserContext? userContext
        )
    {
        // Enhance filter according to business rules
        switch (postVisibilityMode)
        {
            case PostVisibilityMode.CatalogOnly:
                postFilter.StatusNames = [PostStatus.Active.ToString()];
                break;

            case PostVisibilityMode.UserOwnPosts:
                if (userContext is null)
                {
                    return Result<IReadOnlyList<Post>>.Failure(ErrorType.Unauthorized,
                        ErrorMessages.User.NotAuthenticated);
                }
                postFilter.StatusNames = [PostStatus.Active.ToString(), PostStatus.Inactive.ToString()];
                postFilter.UserId = userContext.UserId;
                break;
        }

        if ((userContext is null || !userContext.IsAdmin) && postVisibilityMode == PostVisibilityMode.AdminView)
        {
            return Result<IReadOnlyList<Post>>.Failure(ErrorType.Unauthorized, ErrorMessages.User.NotAuthorized);
        }

        // Get filtered posts
        IReadOnlyList<Post> posts = await postRepo.GetAllPostsAsync(postFilter);
        
        return Result<IReadOnlyList<Post>>.Success(posts);
    }

    /// <inheritdoc />
    public async Task<Result<Post>> GetPostByIdAsync(long postId, UserContext? userContext)
    {
        Post? post = await postRepo.GetPostByIdAsync(postId);
        if (post is null)
        {
            return Result<Post>.Failure(ErrorType.NotFound,ErrorMessages.NotFound(EntityName));
        }
        
        // Apply visibility business rules
        switch (post.Status)
        {
            case PostStatus.Active:
                return Result<Post>.Success(post);

            case PostStatus.Inactive:
                if (userContext is null)
                    return Result<Post>.Failure(ErrorType.Unauthorized, ErrorMessages.User.NotAuthenticated);

                if (post.OwnerUserId != userContext.UserId)
                    return Result<Post>.Failure(ErrorType.Forbidden, ErrorMessages.User.NotAuthorized);

                return Result<Post>.Success(post);

            case PostStatus.Deleted:
                if (userContext is null || !userContext.IsAdmin)
                    return Result<Post>.Failure(ErrorType.NotFound, ErrorMessages.NotFound(EntityName));

                return Result<Post>.Success(post);
            default:
                return Result<Post>.Failure(ErrorType.Unknown,
                    ErrorMessages.UnknownErrorDuringAction("search", "post"));
        }
    }

    /// <inheritdoc />
    public async Task<Result<Post>> CreatePostAsync(Post post, UserContext userContext)
    {
        // Validate business rules of transaction
        Result businessRulesValidationResult = await businessRulesValidator.ValidateNewPostAsync(post, userContext);
        if (businessRulesValidationResult.IsFailure)
        {
            return Result<Post>.Failure(businessRulesValidationResult.Errors!);
        }
        
        // Complete post information
        post.Status = PostStatus.Active;
        post.OwnerUserId = userContext.UserId;
        post.CreatedAt = DateTime.Now;
        
        // Add to database
        postRepo.AddPost(post);
        if (await postRepo.SaveChangesAsync())
        {
            return await postRepo.PostExistsAsync(post.Id) 
                ? Result<Post>.Failure(ErrorType.Persistence, ErrorMessages.SavedButNotRetrieved(EntityName))
                : Result<Post>.Success(post);
        }
        
        return  Result<Post>.Failure(ErrorType.Persistence,ErrorMessages.ErrorWhenSavingToDb(EntityName));
    }

    /// <inheritdoc />
    public async Task<Result> UpdatePostDetailsAsync(long postId, PostDetailsUpdate newDetails, UserContext userContext)
    {
        // Retrieve the existing post
        Post? post = await postRepo.GetPostByIdAsync(postId);
        if (post == null)
        {
            return Result.Failure(ErrorType.NotFound,ErrorMessages.NotFound(EntityName));
        }
        
        // Control authorization to modify the post
        if (!authorizationBusinessRules.CanManagePost(post, userContext))
        {
            return Result.Failure(ErrorType.Forbidden,ErrorMessages.User.NotAuthorized);
        }
        
        if (!userContext.IsAdmin)
        {
            if (post.Status == PostStatus.Deleted)
            {
                return Result.Failure(ErrorType.NotFound, ErrorMessages.NotFound(EntityName));
            }

            if (post.OwnerUserId != userContext.UserId)
            {
                return Result.Failure(ErrorType.Forbidden, ErrorMessages.User.NotAuthorized);
            }
        }
        
        // Validate business rules
        Result businessRulesValidationResult = await businessRulesValidator.ValidatePostUpdateAsync(
            post, 
            newDetails.Title,
            userContext);
        if (businessRulesValidationResult.IsFailure)
        {
            return businessRulesValidationResult;
        }
        
        // Apply modifications
        if (newDetails.Title is not null)
        {
            post.Title = newDetails.Title;
        }

        if (newDetails.Description is not null)
        {
            post.Description = newDetails.Description;
        }

        if (newDetails.Price.HasValue)
        {
            post.Price = newDetails.Price.Value;
        }
        
        if (newDetails.Type is not null)
        {
            post.Type = Enum.Parse<PostType>(newDetails.Type, true);
        }
        
        if (newDetails.PostCategoryId.HasValue)
        {
            post.PostCategoryId = newDetails.PostCategoryId.Value;
        }
        
        post.UpdatedAt = DateTime.Now;

        // Save updates
        return await postRepo.SaveChangesAsync()
            ? Result.Success()
            : Result.Failure(ErrorType.Persistence,ErrorMessages.ErrorWhenSavingToDb(EntityName));
    }

    /// <inheritdoc />
    public async Task<Result> ChangePostStatusAsync(long postId, UserContext userContext, PostStatus targetStatus)
    {
        // Retrieve the existing post
        Post? post = await postRepo.GetPostByIdAsync(postId);
        if (post == null)
        {
            return Result.Failure(ErrorType.NotFound,ErrorMessages.NotFound(EntityName));
        }
        
        // Control authorization to modify the post
        if (!authorizationBusinessRules.CanManagePost(post, userContext))
        {
            return Result.Failure(ErrorType.Forbidden,ErrorMessages.User.NotAuthorized);
        }
        
        if (!userContext.IsAdmin)
        {
            if (post.Status == PostStatus.Deleted)
            {
                return Result.Failure(ErrorType.NotFound, ErrorMessages.NotFound(EntityName));
            }

            if (post.OwnerUserId != userContext.UserId)
            {
                return Result.Failure(ErrorType.Forbidden, ErrorMessages.User.NotAuthorized);
            }
        }
        
        // Validate business rules 
        Result businessRulesValidationResult = businessRulesValidator.ValidatePostStatusChangeAsync(post, targetStatus);
        if (businessRulesValidationResult.IsFailure)
        {
            return businessRulesValidationResult;
        }
        
        // Apply modification
        post.Status = targetStatus;
        
        // Save updates
        return await postRepo.SaveChangesAsync()
            ? Result.Success()
            : Result.Failure(ErrorType.Persistence,ErrorMessages.ErrorWhenSavingToDb(EntityName));
    }

    /// <inheritdoc />
    public async Task<Result> DeletePostAsync(long postId, UserContext userContext)
    {
        // Retrieve the existing post
        Post? post = await postRepo.GetPostByIdAsync(postId);
        if (post == null)
        {
            return Result.Failure(ErrorType.NotFound,ErrorMessages.NotFound(EntityName));
        }
        
        // Control authorization to delete the post
        if (!authorizationBusinessRules.CanManagePost(post, userContext))
        {
            return Result.Failure(ErrorType.Forbidden,ErrorMessages.User.NotAuthorized);
        }
        
        // Validate business rules
        Result businessRulesValidationResult =
            await businessRulesValidator.ValidatePostDeletionAsync(post,userContext);
        if (businessRulesValidationResult.IsFailure)
        {
            return businessRulesValidationResult;
        }
                
        // Deletion logic
        bool isRelatedToTransaction = await transactionRepo.IsPostInTransactionAsync(postId);
        if (isRelatedToTransaction)
        {
            post.Status = PostStatus.Deleted;
        }
        else
        {
            postRepo.DeletePost(post);
        }
        
        // Save to database
        return await postRepo.SaveChangesAsync()
            ? Result.Success()
            : Result.Failure(ErrorType.Persistence,ErrorMessages.ErrorWhenSavingToDb(EntityName));
    }
}
    
   