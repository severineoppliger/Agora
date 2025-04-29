using Agora.API.DTOs.Post;
using Agora.API.DTOs.PostCategory;
using Agora.API.DTOs.Transaction;
using Agora.API.DTOs.TransactionStatus;
using Agora.API.DTOs.User;

namespace Agora.API.InputValidation.Interfaces;

public interface IInputValidator
{
    public Task<List<string>> ValidateCreateUserDtoAsync(CreateUserDto dto);
    public Task<List<string>> ValidateCreatePostCategoryDtoAsync(CreatePostCategoryDto dto);
    public Task<List<string>> ValidateCreatePostDtoAsync(CreatePostDto dto);
    public Task<List<string>> ValidateCreateTransactionStatusDtoAsync(CreateTransactionStatusDto dto);
    public Task<List<string>> ValidateCreateTransactionDtoAsync(CreateTransactionDto dto);
}