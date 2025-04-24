using Agora.Core.Interfaces;
using Agora.Core.Models;
using Agora.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Agora.Infrastructure.Repositories;

public class TransactionStatusRepository(AgoraDbContext context): ITransactionStatusRepository
{
    public async Task<IReadOnlyList<TransactionStatus>> GetAllTransactionStatusAsync()
    {
        return await context.TransactionStatus.ToListAsync();
    }

    public async Task<TransactionStatus?> GetTransactionStatusByIdAsync(long id)
    {
        return await context.TransactionStatus.FindAsync(id);
    }

    public void AddTransactionStatus(TransactionStatus transactionStatus)
    {
        context.TransactionStatus.Add(transactionStatus);
    }

    public void DeleteTransactionStatus(TransactionStatus transactionStatus)
    {
        context.TransactionStatus.Remove(transactionStatus);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }
}