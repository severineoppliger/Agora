using Agora.API.DTOs.Post;
using Agora.API.DTOs.PostCategory;
using Agora.API.DTOs.Transaction;
using Agora.API.DTOs.TransactionStatus;
using Agora.API.DTOs.User;
using Agora.API.InputValidation.Interfaces;
using Agora.Core.Constants;
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
            inputErrors.Add(ErrorMessages.AlreadyExists("Username", dto.UserName));
        }
        
        if (await userManager.FindByEmailAsync(dto.Email) is not null)
        {
            inputErrors.Add(ErrorMessages.User.EmailAlreadyRegistered(dto.Email));
        }
        
        return inputErrors;
    }
    
    public async Task<List<string>> ValidateInputPostCategoryDtoAsync(BaseInputPostCategoryDto dto, string? currentName = null)
    {
        List<string> inputErrors = new();

        if (currentName != null && dto.Name.Equals(currentName))
        {
            inputErrors.Add(ErrorMessages.NewMustBeDifferentFromCurrent("post category name"));
            return inputErrors;
        }
        
        if (await postCategoryRepo.NameExistsAsync(dto.Name))
        {
            inputErrors.Add(ErrorMessages.AlreadyExists("post category name", dto.Name));
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
            inputErrors.Add(ErrorMessages.IsInvalid("type", type));
        }
        
        if (dto is UpdatePostDto updatePostDto && !Enum.TryParse<PostStatus>(updatePostDto.Status, true, out _))
        {
            inputErrors.Add(ErrorMessages.IsInvalid("post status", updatePostDto.Status));
        }
            
        if (!await postCategoryRepo.PostCategoryExistsAsync(postCategoryId))
        {
            inputErrors.Add(ErrorMessages.RelatedEntityDoesNotExist("post category", postCategoryId));
        }

        return inputErrors;
    }

    public async Task<List<string>> ValidateInputTransactionStatusDtoAsync(BaseInputTransactionStatusDto dto, string? currentName = null)
    {
        List<string> inputErrors = new();

        if (currentName != null && dto.Name.Equals(currentName))
        {
            inputErrors.Add(ErrorMessages.NewMustBeDifferentFromCurrent("transaction status name"));
            return inputErrors;
        }
        
        if (await transactionStatusRepo.NameExistsAsync(dto.Name))
        {
            inputErrors.Add(ErrorMessages.AlreadyExists("transaction status name", dto.Name));
        }
        
        if (dto.IsSuccess & !dto.IsFinal)
        {
            inputErrors.Add(ErrorMessages.TransactionStatus.MustBeFinalIfSuccess);
        }

        return inputErrors;
    }

    public async Task<List<string>> ValidateCreateTransactionDtoAsync(CreateTransactionDto dto)
    {
        (_,_, long? postId, string buyerId, string sellerId, _) = dto;
        
        List<string> inputErrors = new();
        
        if (postId is not null && !await postRepo.PostExistsAsync(postId.Value))
        {
            inputErrors.Add(ErrorMessages.RelatedEntityDoesNotExist("post", postId));
        }
        
        if (await userManager.FindByIdAsync(buyerId) is null)
        {
            inputErrors.Add(ErrorMessages.User.BuyerOrSellerDoesNotExist("buyer",buyerId));
        }
        
        if (await userManager.FindByIdAsync(sellerId) is null)
        {
            inputErrors.Add(ErrorMessages.User.BuyerOrSellerDoesNotExist("seller",sellerId));
        }
        
        return inputErrors;
    }
}