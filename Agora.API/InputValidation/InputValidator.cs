using Agora.API.DTOs.Post;
using Agora.API.DTOs.PostCategory;
using Agora.API.DTOs.Transaction;
using Agora.API.DTOs.TransactionStatus;
using Agora.API.DTOs.User;
using Agora.API.InputValidation.Interfaces;
using Agora.Core.Enums;
using Agora.Core.Interfaces;

namespace Agora.API.InputValidation;

public class InputValidator(
    IUserRepository userRepo,
    IPostCategoryRepository postCategoryRepo,
    IPostRepository postRepo,
    ITransactionStatusRepository transactionStatusRepo): IInputValidator
{
    public Task<List<string>> ValidateInputUserDtoAsync(CreateUserDto dto)
    {
        throw new NotImplementedException();
        // TODO
    }
    
    public async Task<List<string>> ValidateInputPostCategoryDtoAsync(BaseInputPostCategoryDto dto, string? currentName = null)
    {
        List<string> inputErrors = new();

        if (string.IsNullOrWhiteSpace(dto.Name))
            inputErrors.Add("Post category name is required.");

        if (currentName != null && dto.Name.Equals(currentName))
        {
            inputErrors.Add("Post category name must be different from the current name.");
            return inputErrors;
        }
        
        if (await postCategoryRepo.NameExistsAsync(dto.Name))
            inputErrors.Add($"The post category name '{dto.Name}' already exists. Name must be unique.");

        return inputErrors;
    }

    public async Task<List<string>> ValidateInputPostDtoAsync(BaseInputPostDto dto)
    {
        (string title, string description, int price, string type, long postCategoryId) = dto;
        
        List<string> inputErrors = new();

        if (string.IsNullOrWhiteSpace(title))
            inputErrors.Add("Post title is required.");
        
        if (string.IsNullOrWhiteSpace(description))
            inputErrors.Add("Post description is required.");
        
        if (price <= 0)
            inputErrors.Add($"Price must be positive, but {price} was given.");
        
        if (!Enum.TryParse<PostType>(type, true, out _))
            inputErrors.Add($"Post type '{type}' is invalid.");
        
        if (dto is UpdatePostDto updatePostDto && !Enum.TryParse<PostStatus>(updatePostDto.Status, true, out _))
            inputErrors.Add($"Post status '{type}' is invalid.");
            
        if (!await postCategoryRepo.PostCategoryExistsAsync(postCategoryId))
            inputErrors.Add($"Related post category {postCategoryId} doesn't exist.");

        return inputErrors;
    }

    public async Task<List<string>> ValidateInputTransactionStatusDtoAsync(BaseInputTransactionStatusDto dto, string? currentName = null)
    {
        List<string> inputErrors = new();

        if (string.IsNullOrWhiteSpace(dto.Name))
            inputErrors.Add("Transaction status name is required.");

        if (currentName != null && dto.Name.Equals(currentName))
        {
            inputErrors.Add("Transaction status name must be different from the current name.");
            return inputErrors;
        }
        
        if (await transactionStatusRepo.NameExistsAsync(dto.Name))
            inputErrors.Add($"The transaction status name '{dto.Name}' already exists. Name must be unique.");

        return inputErrors;
    }

    public async Task<List<string>> ValidateInputTransactionDtoAsync(BaseInputTransactionDto dto)
    {
        (int price, long? postId, long transactionStatusId, long buyerId, long sellerId) = dto;
        
        List<string> inputErrors = new();
        
        if (price <= 0)
            inputErrors.Add($"Price must be positive, but {price} was given.");
        
        if (postId != null && !await postRepo.PostExistsAsync(postId.Value))
            inputErrors.Add($"Related post {postId} doesn't exist.");
        
        if (!await transactionStatusRepo.TransactionStatusExistsAsync(transactionStatusId))
            inputErrors.Add($"Related transaction status {transactionStatusId} doesn't exist.");
        
        if (!await userRepo.UserExistsAsync(buyerId))
            inputErrors.Add($"Buyer (user with id {buyerId}) doesn't exist.");
        
        if (!await userRepo.UserExistsAsync(sellerId))
            inputErrors.Add($"Seller (user with id {sellerId}) doesn't exist.");
        
        return inputErrors;
    }
}