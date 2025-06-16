using Agora.API.DTOs.Post;
using Agora.API.DTOs.Transaction;
using Agora.API.DTOs.User;
using Agora.API.InputValidation.Interfaces;
using Agora.Core.Common;
using Agora.Core.Enums;
using Agora.Core.Interfaces.Repositories;

namespace Agora.API.InputValidation;

public class InputValidator(
    IUserRepository userRepo,
    IPostCategoryRepository postCategoryRepo,
    IPostRepository postRepo,
    ITransactionStatusRepository transactionStatusRepo): IInputValidator
{
    #region Users

    public async Task<InputValidationResult> ValidateRegisterDtoAsync(RegisterDto dto)
    {
        InputValidationResult result = new InputValidationResult();
        
        if (await userRepo.GetUserByUsernameAsync(dto.UserName) is not null)
        {
            result.Errors.Add(ErrorMessages.AlreadyExists("Username", dto.UserName));
        }
        
        if (await userRepo.GetUserByEmailAsync(dto.Email) is not null)
        {
            result.Errors.Add(ErrorMessages.User.EmailAlreadyRegistered(dto.Email));
        }
        
        return result;
    }
    
    #endregion
    
    #region Posts


    public async Task<InputValidationResult> ValidateCreatePostDtoAsync(CreatePostDto dto)
    {
        InputValidationResult result = new InputValidationResult();
        
        if (!Enum.TryParse<PostType>(dto.Type, true, out _))
        {
            result.Errors.Add(ErrorMessages.IsInvalid("type", dto.Type));
        }

            
        if (!await postCategoryRepo.PostCategoryExistsAsync(dto.PostCategoryId))
        {
            result.Errors.Add(ErrorMessages.RelatedEntityDoesNotExist("post category", dto.PostCategoryId));
        }

        return result;
    }

    public async Task<InputValidationResult> ValidateUpdatePostDtoAsync(UpdatePostDetailsDto dto)
    {
        InputValidationResult result = new InputValidationResult();
        
        if (dto.Type is not null && !Enum.TryParse<PostType>(dto.Type, true, out _))
        {
            result.Errors.Add(ErrorMessages.IsInvalid("type", dto.Type));
        }

            
        if (dto.PostCategoryId is not null && !await postCategoryRepo.PostCategoryExistsAsync(dto.PostCategoryId.Value))
        {
            result.Errors.Add(ErrorMessages.RelatedEntityDoesNotExist("post category", dto.PostCategoryId));
        }

        return result;
    }

    #endregion

    #region Transaction
    /// <summary>
    /// Validates the input for a new transaction request.
    /// Ensures that the involved users and the optional linked post exist in database
    /// </summary>
    public async Task<InputValidationResult> ValidateCreateTransactionDtoAsync(CreateTransactionDto dto)
    {
        InputValidationResult result = new InputValidationResult();
        
        if (dto.PostId is not null && !await postRepo.PostExistsAsync(dto.PostId.Value))
        {
            result.Errors.Add(ErrorMessages.RelatedEntityDoesNotExist("post", dto.PostId));
        }
        
        if (!await userRepo.UserExistsAsync(dto.BuyerId))
        {
            result.Errors.Add(ErrorMessages.User.BuyerOrSellerDoesNotExist("buyer",dto.BuyerId));
        }
        
        if (!await userRepo.UserExistsAsync(dto.SellerId))
        {
            result.Errors.Add(ErrorMessages.User.BuyerOrSellerDoesNotExist("seller",dto.SellerId));
        }
        
        return result;
    }

    public async Task<InputValidationResult> ValidateUpdateTransactionDetailsDtoAsync(UpdateTransactionDetailsDto dto)
    {
        InputValidationResult result = new InputValidationResult();
        
        if (dto.PostId is not null && !await postRepo.PostExistsAsync(dto.PostId.Value))
        {
            result.Errors.Add(ErrorMessages.RelatedEntityDoesNotExist("post", dto.PostId));
        }

        return result;
    }

    /// <summary>
    /// Validates the input for a transaction status change request.
    /// Ensures that the provided transaction status exists within the TransactionStatusEnum.
    /// </summary>
    public InputValidationResult ValidateChangeTransactionStatusDto(ChangeTransactionStatusDto dto)
    {
        InputValidationResult result = new InputValidationResult();
        
        // Status should have existing value
        if (!Enum.IsDefined(typeof(TransactionStatusEnum), dto.NewStatus))
        {
            result.Errors.Add(ErrorMessages.IsInvalid("transaction status", dto.NewStatus.ToString()));
        }

        return result;
    }
    #endregion

}