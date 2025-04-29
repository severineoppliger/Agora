using Agora.API.DTOs.Post;
using Agora.API.DTOs.PostCategory;
using Agora.API.DTOs.Transaction;
using Agora.API.DTOs.TransactionStatus;
using Agora.API.DTOs.User;
using Agora.API.InputValidation.Interfaces;
using Agora.Core.Interfaces;

namespace Agora.API.InputValidation;

public class InputValidator(IPostRepository postRepo, ITransactionStatusRepository transactionStatusRepo, IUserRepository userRepo): IInputValidator
{
    public Task<List<string>> ValidateInputUserDtoAsync(CreateUserDto dto)
    {
        throw new NotImplementedException();
        // TODO
    }

    public Task<List<string>> ValidateInputPostCategoryDtoAsync(CreatePostCategoryDto dto)
    {
        throw new NotImplementedException();
        // TODO
    }

    public Task<List<string>> ValidateInputPostDtoAsync(CreatePostDto dto)
    {
        throw new NotImplementedException();
        // TODO
    }

    public Task<List<string>> ValidateInputTransactionStatusDtoAsync(CreateTransactionStatusDto dto)
    {
        throw new NotImplementedException();
        // TODO
    }

    public async Task<List<string>> ValidateInputTransactionDtoAsync(BaseInputTransactionDto dto)
    {
        (int price, long? postId, long transactionStatusId, long buyerId, long sellerId) = dto;
        
        var errors = new List<string>();
        if (price <= 0)
            errors.Add($"Price must be positive, but {price} was given.");
        if (postId != null && !await postRepo.PostExistsAsync(postId.Value))
            errors.Add($"Related post {postId} doesn't exist.");
        if (!await transactionStatusRepo.TransactionStatusExistsAsync(transactionStatusId))
            errors.Add($"Related transaction status {transactionStatusId} doesn't exist.");
        if (!await userRepo.UserExistsAsync(buyerId))
            errors.Add($"Buyer (user with id {buyerId}) doesn't exist.");
        if (!await userRepo.UserExistsAsync(sellerId))
            errors.Add($"Seller (user with id {sellerId}) doesn't exist.");
        return errors;
    }
}