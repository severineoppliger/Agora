using Agora.Core.Enums;
using Agora.Core.Interfaces.QueryParameters;
using Agora.Core.Interfaces.Repositories;
using Agora.Core.Models;
using Agora.Core.Models.Entities;
using Agora.Core.Shared;
using Agora.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Agora.Infrastructure.Repositories;

public class TransactionStatusRepository(AgoraDbContext context): ITransactionStatusRepository
{
    public async Task<IReadOnlyList<TransactionStatus>> GetAllTransactionStatusAsync(ITransactionStatusQueryParameters queryParameters)
    {
        IQueryable<TransactionStatus> transactionStatus = context.TransactionStatus;

        if (!string.IsNullOrWhiteSpace(queryParameters.NameOrDescription))
        {
            transactionStatus = transactionStatus.Where(ts => ts.Name.Contains(queryParameters.NameOrDescription) || ts.Description.Contains(queryParameters.NameOrDescription));
        }

        if (queryParameters.IsFinal.HasValue)
        {
            transactionStatus = transactionStatus.Where(ts => ts.IsFinal == queryParameters.IsFinal);
        }

        if (queryParameters.IsSuccess.HasValue)
        {
            transactionStatus = transactionStatus.Where(ts => ts.IsSuccess == queryParameters.IsSuccess);
        }
        
        transactionStatus = ApplySorting(transactionStatus, queryParameters);
        
        return await transactionStatus.ToListAsync();
    }

    public async Task<TransactionStatus?> GetTransactionStatusByIdAsync(long id)
    {
        return await context.TransactionStatus
            .Include(ts => ts.Transactions)
                .ThenInclude(t => t.Post)
            .Include(ts => ts.Transactions)
                .ThenInclude(t => t.Buyer)
            .Include(ts => ts.Transactions)
                .ThenInclude(t => t.Seller)
            .FirstOrDefaultAsync(ts => ts.Id == id);
    }

    public async Task<TransactionStatus?> GetTransactionStatusByEnumAsync(TransactionStatusEnum statusEnum)
    {
        return await context.TransactionStatus
            .Include(ts =>ts.Transactions)
            .FirstOrDefaultAsync(ts => ts.EnumValue == statusEnum);
    }

    public async Task<long> GetIdByEnumAsync(TransactionStatusEnum statusEnum)
    {
        TransactionStatus? transactionStatus = await context.TransactionStatus.FirstOrDefaultAsync(s =>
            s.EnumValue == statusEnum);
        if (transactionStatus is null)
            throw new InvalidOperationException(ErrorMessages.NotFound("transactionStatus", statusEnum.ToString()));
        return transactionStatus.Id;
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }

    public Task<bool> NameExistsAsync(string name)
    {
        return context.TransactionStatus.AnyAsync(ts => ts.Name == name);
    }

    public IQueryable<TransactionStatus> ApplySorting(IQueryable<TransactionStatus> query, ITransactionStatusQueryParameters queryParams)
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