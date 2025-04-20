using Agora.Core.Models;

namespace Agora.Core.Interfaces;

public interface ITransactionStatusRepository
{
    Task<IReadOnlyList<TransactionStatus>> GetAllTransactionStatusAsync();
    Task<TransactionStatus?> GetTransactionStatusByIdAsync(long id);
    void AddTransactionStatus(TransactionStatus transactionStatus);
    void UpdateTransactionStatus(TransactionStatus transactionStatus);
    void DeleteTransactionStatus(TransactionStatus transactionStatus);
    bool TransactionStatusExists(long id);
    Task<bool> SaveChangesAsync();
}