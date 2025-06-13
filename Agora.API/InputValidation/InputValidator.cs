using Agora.API.DTOs.Post;
using Agora.API.DTOs.PostCategory;
using Agora.API.DTOs.Transaction;
using Agora.API.DTOs.TransactionStatus;
using Agora.API.DTOs.User;
using Agora.API.InputValidation.Interfaces;
using Agora.Core.Enums;
using Agora.Core.Interfaces.Repositories;
using Agora.Core.Models;
using Microsoft.AspNetCore.Identity;

namespace Agora.API.InputValidation;

public class InputValidator(
    UserManager<AppUser> userManager,
    IPostCategoryRepository postCategoryRepo,
    IPostRepository postRepo,
    ITransactionStatusRepository transactionStatusRepo): IInputValidator
{
    public async Task<List<string>> ValidateInputRegisterDtoAsync(RegisterDto dto)
    {
        List<string> inputErrors = new();
        
        if (await userManager.FindByNameAsync(dto.UserName) is not null)
        {
            inputErrors.Add($"The username '{dto.UserName}' is already taken. Username must be unique.");
        }
        
        if (await userManager.FindByEmailAsync(dto.Email) is not null)
        {
            inputErrors.Add($"An account with the email '{dto.Email}' already exists. Email must be unique.");
        }
        
        return inputErrors;
    }
    
    public async Task<List<string>> ValidateInputPostCategoryDtoAsync(BaseInputPostCategoryDto dto, string? currentName = null)
    {
        List<string> inputErrors = new();

        if (currentName != null && dto.Name.Equals(currentName))
        {
            inputErrors.Add("Post category name must be different from the current name.");
            return inputErrors;
        }
        
        if (await postCategoryRepo.NameExistsAsync(dto.Name))
        {
            inputErrors.Add($"The post category name '{dto.Name}' already exists. Name must be unique.");
        }

        return inputErrors;
    }

    public async Task<List<string>> ValidateInputPostDtoAsync(BaseInputPostDto dto)
    {
        string type = dto.Type;
        long postCategoryId = dto.PostCategoryId;
        
        List<string> inputErrors = new();
        
        if (!Enum.TryParse<PostType>(type, true, out _))
        {
            inputErrors.Add($"Post type '{type}' is invalid.");
        }
        
        if (dto is UpdatePostDto updatePostDto && !Enum.TryParse<PostStatus>(updatePostDto.Status, true, out _))
        {
            inputErrors.Add($"Post status '{type}' is invalid.");
        }
            
        if (!await postCategoryRepo.PostCategoryExistsAsync(postCategoryId))
        {
            inputErrors.Add($"Related post category {postCategoryId} doesn't exist.");
        }

        return inputErrors;
    }

    public async Task<List<string>> ValidateInputTransactionStatusDtoAsync(BaseInputTransactionStatusDto dto, string? currentName = null)
    {
        List<string> inputErrors = new();

        if (currentName != null && dto.Name.Equals(currentName))
        {
            inputErrors.Add("Transaction status name must be different from the current name.");
            return inputErrors;
        }
        
        if (await transactionStatusRepo.NameExistsAsync(dto.Name))
        {
            inputErrors.Add($"The transaction status name '{dto.Name}' already exists. Name must be unique.");
        }
        
        if (dto.IsSuccess & !dto.IsFinal)
        {
            inputErrors.Add("Transaction status must be final if it's a success.");
        }

        return inputErrors;
    }

    public async Task<List<string>> ValidateCreateTransactionDtoAsync(CreateTransactionDto dto)
    {
        (_,_, long? postId, string buyerId, string sellerId, _) = dto;
        
        List<string> inputErrors = new();
        
        if (postId != null && !await postRepo.PostExistsAsync(postId.Value))
        {
            inputErrors.Add($"Related post {postId} doesn't exist.");
        }
        
        if (await userManager.FindByIdAsync(buyerId) is null)
        {
            inputErrors.Add($"Buyer (user with id {buyerId}) doesn't exist.");
        }
        
        if (await userManager.FindByIdAsync(sellerId) is null)
        {
            inputErrors.Add($"Seller (user with id {sellerId}) doesn't exist.");
        }
        
        return inputErrors;
    }
}