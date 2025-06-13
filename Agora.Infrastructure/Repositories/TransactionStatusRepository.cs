using Agora.Core.Interfaces.Filters;
using Agora.Core.Interfaces.Repositories;
using Agora.Core.Models;
using Agora.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Agora.Infrastructure.Repositories;

public class TransactionStatusRepository(AgoraDbContext context): ITransactionStatusRepository
{
    public async Task<IReadOnlyList<TransactionStatus>> GetAllTransactionStatusAsync(ITransactionStatusFilter filter)
    {
        IQueryable<TransactionStatus> transactionStatus = context.TransactionStatus;

        if (!string.IsNullOrWhiteSpace(filter.NameOrDescription))
        {
            transactionStatus = transactionStatus.Where(ts => ts.Name.Contains(filter.NameOrDescription) || ts.Description.Contains(filter.NameOrDescription));
        }

        if (filter.IsFinal.HasValue)
        {
            transactionStatus = transactionStatus.Where(ts => ts.IsFinal == filter.IsFinal);
        }

        if (filter.IsSuccess.HasValue)
        {
            transactionStatus = transactionStatus.Where(ts => ts.IsSuccess == filter.IsSuccess);
        }
        
        transactionStatus = ApplySorting(transactionStatus, filter);
        
        return await transactionStatus.ToListAsync();
    }

    public async Task<TransactionStatus?> GetTransactionStatusByIdAsync(long id)
    {
        return await context.TransactionStatus
            .Include(ts => ts.Transactions)
            .FirstOrDefaultAsync(ts => ts.Id == id);
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

    public async Task<bool> TransactionStatusExistsAsync(long id)
    {
        return await context.TransactionStatus.AnyAsync(ts => ts.Id == id);
    }

    public Task<bool> NameExistsAsync(string name)
    {
        return context.TransactionStatus.AnyAsync(ts => ts.Name == name);
    }

    public IQueryable<TransactionStatus> ApplySorting(IQueryable<TransactionStatus> query, ITransactionStatusFilter queryParams)
    {
        query = queryParams.SortBy?.ToLower() switch
        {
            "id" => queryParams.SortDesc ? query.OrderByDescending(ts => ts.Id) : query.OrderBy(ts => ts.Id),
            "name" => queryParams.SortDesc ? query.OrderByDescending(ts => ts.Name) : query.OrderBy(ts => ts.Name),
            "isfinal" => queryParams.SortDesc ? query.OrderByDescending(ts => ts.IsFinal) : query.OrderBy(ts => ts.IsFinal),
            _ => query.OrderBy(u => u.Id)
        };
        return query;
    }
}