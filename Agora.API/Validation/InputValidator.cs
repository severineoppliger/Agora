using Agora.API.DTOs.Post;
using Agora.API.DTOs.Transaction;
using Agora.API.DTOs.TransactionStatus;
using Agora.API.DTOs.User;
using Agora.API.Validation.Interfaces;
using Agora.Core.Enums;
using Agora.Core.Extensions;
using Agora.Core.Interfaces.Repositories;
using Agora.Core.Shared;

namespace Agora.API.Validation;

/// <summary>
/// Default implementation of <see cref="IInputValidator"/>.
/// Performs validation of user input for user registration, post management,
/// and transaction-related operations.
/// </summary>
public class InputValidator(
    IUserRepository userRepo,
    IPostCategoryRepository postCategoryRepo,
    IPostRepository postRepo): IInputValidator
{
    #region Users

    /// <inheritdoc />
    public async Task<InputValidationResult> ValidateRegisterDtoAsync(RegisterUserDto userDto)
    {
        InputValidationResult result = new InputValidationResult();
        
        if (await userRepo.GetUserByUsernameAsync(userDto.UserName) is not null)
        {
            result.Errors.Add(ErrorMessages.AlreadyExists("Username", userDto.UserName));
        }
        
        if (await userRepo.GetUserByEmailAsync(userDto.Email) is not null)
        {
            result.Errors.Add(ErrorMessages.User.EmailAlreadyRegistered(userDto.Email));
        }
        
        if (userDto.Password != userDto.Password.Trim())
        {
            result.Errors.Add(ErrorMessages.User.PasswordWithoutSpaceBeforeOrAfter);
        }
        
        return result;
    }
    
    /// <inheritdoc />
    public InputValidationResult ValidateUserId(string id)
    {
        InputValidationResult result = new InputValidationResult();
        
        if (!id.IsGuid())
        {
            result.Errors.Add(ErrorMessages.User.InvalidIdFormat(id));
        }
        
        return result;
    }
    
    
    #endregion
    
    #region Posts

    /// <inheritdoc />
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

    /// <inheritdoc />
    public async Task<InputValidationResult> ValidateUpdatePostDtoAsync(UpdatePostDetailsDto dto)
    {
        InputValidationResult result = new InputValidationResult();

        if (dto.IsEmpty())
        {
            result.Errors.Add(ErrorMessages.EmptyDto);
        }
        
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

    /// <inheritdoc />
    public InputValidationResult ValidateUpdateTransactionStatusDtoAsync(UpdateTransactionStatusDetailsDto dto)
    {
        InputValidationResult result = new InputValidationResult();

        if (dto.IsEmpty())
        {
            result.Errors.Add(ErrorMessages.EmptyDto);
        }

        return result;
    }
    
    #region Transaction
    /// <inheritdoc />
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

    /// <inheritdoc />
    public async Task<InputValidationResult> ValidateUpdateTransactionDetailsDtoAsync(UpdateTransactionDetailsDto dto)
    {
        InputValidationResult result = new InputValidationResult();
        
        if (dto.IsEmpty())
        {
            result.Errors.Add(ErrorMessages.EmptyDto);
        }
        
        if (dto.PostId is not null && !await postRepo.PostExistsAsync(dto.PostId.Value))
        {
            result.Errors.Add(ErrorMessages.RelatedEntityDoesNotExist("post", dto.PostId));
        }

        return result;
    }

    /// <inheritdoc />
    public InputValidationResult ValidateChangeTransactionStatusDto(ChangeTransactionStatusDto dto)
    {
        InputValidationResult result = new InputValidationResult();
        
        // Status should have existing value
        if (!Enum.IsDefined(typeof(TransactionStatusEnum), dto.Status))
        {
            result.Errors.Add(ErrorMessages.IsInvalid("transaction status", dto.Status.ToString()));
        }

        return result;
    }
    #endregion
}