using Agora.Core.BusinessRules.Interfaces;
using Agora.Core.Common;
using Agora.Core.Enums;
using Agora.Core.Interfaces.BusinessServices;
using Agora.Core.Interfaces.Filters;
using Agora.Core.Interfaces.Repositories;
using Agora.Core.Models;

namespace Agora.Core.BusinessServices;

public class PostCategoryService(
    IPostCategoryRepository postCategoryRepo,
    IBusinessRulesValidator businessRulesValidator,
    IAuthorizationBusinessRules authorizationBusinessRules
    ) : IPostCategoryService
{
    private const string EntityName = "post category";

    public async Task<Result<IReadOnlyList<PostCategory>>> GetAllPostCategoriesAsync(IPostCategoryFilter postCategoryFilter)
    {
        IReadOnlyList<PostCategory> postCategories = await postCategoryRepo.GetAllPostCategoriesAsync(postCategoryFilter);
        
        return Result<IReadOnlyList<PostCategory>>.Success(postCategories);
    }

    public async Task<Result<PostCategory>> GetPostCategoryByIdAsync(long postCategoryId)
    {
        PostCategory? postCategory = await postCategoryRepo.GetPostCategoryByIdAsync(postCategoryId);
        if (postCategory is null)
        {
            return Result<PostCategory>.Failure(ErrorType.NotFound,ErrorMessages.NotFound(EntityName));
        }
        
        return Result<PostCategory>.Success(postCategory);
    }

    public async Task<Result<PostCategory>> CreatePostCategoryAsync(PostCategory postCategory)
    {
        // Validate business rules
        Result businessRulesValidationResult = await businessRulesValidator.ValidateNewPostCategoryAsync(postCategory);
        if (businessRulesValidationResult.IsFailure)
        {
            return Result<PostCategory>.Failure(businessRulesValidationResult.Errors!);
        }
        
        // Add to database
        postCategoryRepo.AddPostCategory(postCategory);
        if (await postCategoryRepo.SaveChangesAsync())
        {
            return await postCategoryRepo.PostCategoryExistsAsync(postCategory.Id) 
                ? Result<PostCategory>.Failure(ErrorType.Persistence, ErrorMessages.SavedButNotRetrieved(EntityName))
                : Result<PostCategory>.Success(postCategory);
        }

        return  Result<PostCategory>.Failure(ErrorType.Persistence,ErrorMessages.ErrorWhenSavingToDb(EntityName));
    }

    public async Task<Result> UpdatePostCategoryNameAsync(long postCategoryId, string newName)
    {
        // Retrieve the existing post category
        PostCategory? postCategory = await postCategoryRepo.GetPostCategoryByIdAsync(postCategoryId);
        if (postCategory == null)
        {
            return Result.Failure(ErrorType.NotFound,ErrorMessages.NotFound(EntityName));
        }
        
        // Validate business rules
        Result businessRulesValidationResult = await businessRulesValidator.ValidatePostCategoryUpdateAsync(postCategory, newName);
        if (businessRulesValidationResult.IsFailure)
        {
            return businessRulesValidationResult;
        }
        
        // Apply modification and save to database
        postCategory.Name = newName;
        
        return await postCategoryRepo.SaveChangesAsync()
            ? Result.Success()
            : Result.Failure(ErrorType.Persistence,ErrorMessages.ErrorWhenSavingToDb(EntityName));
    }

    public async Task<Result> DeletePostCategoryAsync(long postCategoryId)
    {
        // Retrieve the existing post category
        PostCategory? postCategory = await postCategoryRepo.GetPostCategoryByIdAsync(postCategoryId);
        if (postCategory == null)
        {
            return Result.Failure(ErrorType.NotFound,ErrorMessages.NotFound(EntityName));
        }
        
        // Validate business rules
        Result businessRulesValidationResult =
            await businessRulesValidator.ValidatePostCategoryDeletionAsync(postCategory);
        if (businessRulesValidationResult.IsFailure)
        {
            return businessRulesValidationResult;
        }
        
        // Delete the entity and save to database
        postCategoryRepo.DeletePostCategory(postCategory);
        return await postCategoryRepo.SaveChangesAsync()
            ? Result.Success()
            : Result.Failure(ErrorType.Persistence,ErrorMessages.ErrorWhenSavingToDb(EntityName));
    }
}