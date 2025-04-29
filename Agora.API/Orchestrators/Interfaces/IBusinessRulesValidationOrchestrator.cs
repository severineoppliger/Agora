using Agora.Core.Models;

namespace Agora.API.Orchestrators.Interfaces;

/* Enhance uncompleted entities with navigation properties to validate the business rules */
public interface IBusinessRulesValidationOrchestrator
{
    public Task<IList<string>> ValidateAndProcessUserAsync(User user);
    public Task<IList<string>> ValidateAndProcessPostCategoryAsync(PostCategory postCategory);
    public Task<IList<string>> ValidateAndProcessPostAsync(Post post);
    public Task<IList<string>> ValidateAndProcessTransactionStatusAsync(TransactionStatus transactionStatus);
    public Task<IList<string>> ValidateAndProcessTransactionAsync(Transaction transaction);
}