using Agora.Core.Interfaces.Filters;
using Agora.Core.Models;

namespace Agora.Core.Interfaces.Repositories;

public interface ITransactionStatusRepository
{
    Task<IReadOnlyList<TransactionStatus>> GetAllTransactionStatusAsync(ITransactionStatusFilter filter);
    Task<TransactionStatus?> GetTransactionStatusByIdAsync(long id);
    void AddTransactionStatus(TransactionStatus transactionStatus);
    void DeleteTransactionStatus(TransactionStatus transactionStatus);
    Task<bool> SaveChangesAsync();
    Task<bool> TransactionStatusExistsAsync(long id);
    Task<bool> NameExistsAsync(string name);
    IQueryable<TransactionStatus> ApplySorting(IQueryable<TransactionStatus> query, ITransactionStatusFilter queryParams);
}