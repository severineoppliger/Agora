using Agora.Core.Interfaces;
using Agora.Core.Models;
using Agora.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Agora.Infrastructure.Repositories;

public class TransactionRepository(AgoraDbContext context) : ITransactionRepository
{
    public async Task<IReadOnlyList<Transaction>> GetAllTransactionsAsync()
    {
        return await context.Transactions.ToListAsync();
    }

    public async Task<Transaction?> GetTransactionByIdAsync(long id)
    {
        return await context.Transactions.FindAsync(id);
    }

    public void AddTransaction(Transaction transaction)
    {
        context.Transactions.Add(transaction);
    }

    public void UpdateTransaction(Transaction transaction)
    {
        context.Entry(transaction).State = EntityState.Modified;
    }

    public void DeleteTransaction(Transaction transaction)
    {
        context.Transactions.Remove(transaction);
    }

    public bool TransactionExists(long id)
    {
        return context.Transactions.Any(t => t.Id == id);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }
}