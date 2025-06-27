using Agora.Core.Commands;
using Agora.Core.Constants;
using Agora.Core.Enums;
using Agora.Core.Interfaces.DomainServices;
using Agora.Core.Interfaces.QueryParameters;
using Agora.Core.Interfaces.Repositories;
using Agora.Core.Models.Entities;
using Agora.Core.Shared;
using Agora.Core.Validation.Interfaces;

namespace Agora.Core.DomainServices;

/// <summary>
/// Default implementation of <see cref="IPostCategoryService"/>.
/// </summary>
public class PostCategoryService(
    IPostCategoryRepository postCategoryRepo,
    IBusinessRulesValidator businessRulesValidator
    ) : IPostCategoryService
{
    private const string EntityName = "post category";

    /// <inheritdoc />
    public async Task<Result<IReadOnlyList<PostCategory>>> GetAllPostCategoriesAsync(
        IPostCategoryQueryParameters queryParams)
    {
        // Validate business rules
        Result businessRulesValidationResult = 
            businessRulesValidator.ValidateSortBy(queryParams.SortBy, SortByOptions.PostCategory);
        if (businessRulesValidationResult.IsFailure)
        {
            return Result<IReadOnlyList<PostCategory>>.Failure(businessRulesValidationResult.Errors!);
        }
        
        // Retrieve in database
        IReadOnlyList<PostCategory> postCategories = await postCategoryRepo.GetAllPostCategoriesAsync(queryParams);
        
        return Result<IReadOnlyList<PostCategory>>.Success(postCategories);
    }

    /// <inheritdoc />
    public async Task<Result<PostCategory>> GetPostCategoryByIdAsync(long postCategoryId)
    {
        PostCategory? postCategory = await postCategoryRepo.GetPostCategoryByIdAsync(postCategoryId);
        return postCategory is null
            ? Result<PostCategory>.Failure(ErrorType.NotFound,ErrorMessages.NotFound(EntityName))
            : Result<PostCategory>.Success(postCategory);
    }

    /// <inheritdoc />
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
            return !await postCategoryRepo.PostCategoryExistsAsync(postCategory.Id) 
                ? Result<PostCategory>.Failure(ErrorType.Persistence, ErrorMessages.SavedButNotRetrieved(EntityName))
                : Result<PostCategory>.Success(postCategory);
        }

        return  Result<PostCategory>.Failure(ErrorType.Persistence,ErrorMessages.ErrorWhenSavingToDb(EntityName));
    }

    /// <inheritdoc />
    public async Task<Result> UpdatePostCategoryDetailsAsync(long postCategoryId, UpdatePostCategoryDetailsCommand newDetails)
    {
        // Retrieve the existing post category
        PostCategory? postCategory = await postCategoryRepo.GetPostCategoryByIdAsync(postCategoryId);
        if (postCategory == null)
        {
            return Result.Failure(ErrorType.NotFound,ErrorMessages.NotFound(EntityName));
        }
        
        // Validate business rules
        Result businessRulesValidationResult = await businessRulesValidator.ValidatePostCategoryUpdateAsync(postCategory, newDetails);
        if (businessRulesValidationResult.IsFailure)
        {
            return businessRulesValidationResult;
        }
        
        // Apply modification and save to database
        postCategory.Name = newDetails.Name;
        
        return await postCategoryRepo.SaveChangesAsync()
            ? Result.Success()
            : Result.Failure(ErrorType.Persistence,ErrorMessages.ErrorWhenSavingToDb(EntityName));
    }

    /// <inheritdoc />
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