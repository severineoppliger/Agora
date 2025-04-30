using Agora.API.DTOs.Post;
using Agora.API.DTOs.PostCategory;
using Agora.API.DTOs.Transaction;
using Agora.API.DTOs.TransactionStatus;
using Agora.API.DTOs.User;

namespace Agora.API.InputValidation.Interfaces;

public interface IInputValidator
{
    public Task<List<string>> ValidateInputUserDtoAsync(CreateUserDto dto);
    public Task<List<string>> ValidateInputPostCategoryDtoAsync(BaseInputPostCategoryDto dto, string? currentName = null);
    public Task<List<string>> ValidateInputPostDtoAsync(CreatePostDto dto);
    public Task<List<string>> ValidateInputTransactionStatusDtoAsync(BaseInputTransactionStatusDto dto, string? currentName = null);
    public Task<List<string>> ValidateInputTransactionDtoAsync(BaseInputTransactionDto dto);
}