using Agora.Core.Enums;
using Agora.Core.Interfaces.QueryParameters;
using Agora.Core.Interfaces.Repositories;
using Agora.Core.Models.Entities;
using Agora.Core.Shared;
using Agora.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Agora.Infrastructure.Repositories;

/// <summary>
/// Default implementation of <see cref="ITransactionStatusRepository"/>.
/// </summary>
public class TransactionStatusRepository(AgoraDbContext context): ITransactionStatusRepository
{
    /// <inheritdoc/>
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

    /// <inheritdoc/>
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

    /// <inheritdoc/>
    public async Task<TransactionStatus?> GetTransactionStatusByEnumAsync(TransactionStatusEnum statusEnum)
    {
        return await context.TransactionStatus
            .Include(ts =>ts.Transactions)
            .FirstOrDefaultAsync(ts => ts.EnumValue == statusEnum);
    }

    /// <inheritdoc/>
    public async Task<long> GetIdByEnumAsync(TransactionStatusEnum statusEnum)
    {
        TransactionStatus? transactionStatus = await context.TransactionStatus.FirstOrDefaultAsync(s =>
            s.EnumValue == statusEnum);
        if (transactionStatus is null)
            throw new InvalidOperationException(ErrorMessages.NotFound("transactionStatus", statusEnum.ToString()));
        return transactionStatus.Id;
    }

    /// <inheritdoc/>
    public async Task<bool> SaveChangesAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }

    /// <inheritdoc/>
    public Task<bool> NameExistsAsync(string name)
    {
        return context.TransactionStatus.AnyAsync(ts => ts.Name == name);
    }

    /// <summary>
    /// Applies sorting to the given <see cref="IQueryable{TransactionStatus}"/> based on the specified query parameters.
    /// </summary>
    /// <param name="query">The queryable collection of <see cref="TransactionStatus"/> to sort.</param>
    /// <param name="queryParams">The sorting parameters specifying the property and order (ascending/descending).</param>
    /// <returns>The sorted <see cref="IQueryable{TransactionStatus}"/>.</returns>
    private IQueryable<TransactionStatus> ApplySorting(IQueryable<TransactionStatus> query, ITransactionStatusQueryParameters queryParams)
    {
        query = queryParams.SortBy?.ToLower() switch
        {
            "id" => queryParams.SortDesc ? query.OrderByDescending(ts => ts.Id) : query.OrderBy(ts => ts.Id),
            "name" => queryParams.SortDesc ? query.OrderByDescending(ts => ts.Name) : query.OrderBy(ts => ts.Name),
            "isfinal" => queryParams.SortDesc ? query.OrderByDescending(ts => ts.IsFinal) : query.OrderBy(ts => ts.IsFinal),
            "issuccess" => queryParams.SortDesc ? query.OrderByDescending(ts => ts.IsSuccess) : query.OrderBy(ts => ts.IsSuccess),
            _ => query.OrderBy(u => u.Id)
        };
        return query;
    }
}