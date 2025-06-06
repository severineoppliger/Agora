using Agora.Core.Models;

namespace Agora.Core.Interfaces;

public interface ITransactionRepository
{
    Task<IReadOnlyList<Transaction>> GetAllTransactionsAsync(ITransactionFilter filter, string? userId = null);
    Task<Transaction?> GetTransactionByIdAsync(long id);
    void AddTransaction(Transaction transaction);
    void DeleteTransaction(Transaction transaction);
    Task<bool> SaveChangesAsync();
    IQueryable<Transaction> ApplySorting(IQueryable<Transaction> query, ITransactionFilter queryParams);
}