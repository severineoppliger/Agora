using Agora.Core.Interfaces;
using Agora.Core.Models;
using Agora.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Agora.Infrastructure.Repositories;

public class TransactionRepository(AgoraDbContext context) : ITransactionRepository
{
    public async Task<IReadOnlyList<Transaction>> GetAllTransactionsAsync()
    {
        return await context.Transactions
            .Include(t => t.Post)
            .Include(t => t.TransactionStatus)
            .Include(t => t.Buyer)
            .Include(t => t.Seller)
            .ToListAsync();
    }

    public async Task<Transaction?> GetTransactionByIdAsync(long id)
    {
        return await context.Transactions
            .Include(t => t.Post)
                .ThenInclude(post => post!.PostCategory)
            .Include(t => t.TransactionStatus)
            .Include(t => t.Buyer)
            .Include(t => t.Seller)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public void AddTransaction(Transaction transaction)
    {
        context.Transactions.Add(transaction);
    }

    public void DeleteTransaction(Transaction transaction)
    {
        context.Transactions.Remove(transaction);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }
}