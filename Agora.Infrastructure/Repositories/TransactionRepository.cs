using Agora.Core.Interfaces.QueryParameters;
using Agora.Core.Interfaces.Repositories;
using Agora.Core.Models;
using Agora.Core.Models.Entities;
using Agora.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Agora.Infrastructure.Repositories;

public class TransactionRepository(AgoraDbContext context) : ITransactionRepository
{
    public async Task<IReadOnlyList<Transaction>> GetAllTransactionsAsync(ITransactionQueryParameters queryParameters)
    {
        IQueryable<Transaction> transactions = context.Transactions.AsQueryable();

        if (queryParameters.MinPrice.HasValue)
        {
            transactions = transactions.Where(t => t.Price >= queryParameters.MinPrice);
        }

        if (queryParameters.MaxPrice.HasValue)
        {
            transactions = transactions.Where(t => t.Price <= queryParameters.MaxPrice);
        }

        if (!string.IsNullOrWhiteSpace(queryParameters.PostTitle))
        {
            transactions = transactions.Where(t => t.Post != null && t.Post.Title.Contains(queryParameters.PostTitle));
        }

        if (!string.IsNullOrWhiteSpace(queryParameters.TransactionStatusName))
        {
            transactions = transactions.Where(t => t.TransactionStatus != null && t.TransactionStatus.Name.Contains(queryParameters.TransactionStatusName));
        }

        if (!string.IsNullOrWhiteSpace(queryParameters.UsersInvolvedUsername))
        {
            transactions = transactions.Where(t => t.Buyer != null && t.Buyer.UserName!.Contains(queryParameters.UsersInvolvedUsername) || t.Seller != null && t.Seller.UserName!.Contains(queryParameters.UsersInvolvedUsername));
        }
        
        transactions = ApplySorting(transactions, queryParameters);
        
        return await transactions
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
            .Include(t=> t.Initiator)
            .Include(t => t.Buyer)
            .Include(t => t.Seller)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public void AddTransaction(Transaction transaction)
    {
        context.Transactions.Add(transaction);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }

    public IQueryable<Transaction> ApplySorting(IQueryable<Transaction> query, ITransactionQueryParameters queryParams)
    {
        query = queryParams.SortBy?.ToLower() switch
        {
            "id" => queryParams.SortDesc ? query.OrderByDescending(t => t.Id) : query.OrderBy(t => t.Id),
            "price" => queryParams.SortDesc ? query.OrderByDescending(t => t.Price) : query.OrderBy(t => t.Price),
            "transactionstatus" => queryParams.SortDesc ? query.OrderByDescending(t => t.TransactionStatusId) : query.OrderBy(t => t.TransactionStatusId),
            "buyer" => queryParams.SortDesc ? query.OrderByDescending(t => t.Buyer!.UserName) : query.OrderBy(t => t.Buyer!.UserName),
            "seller" => queryParams.SortDesc ? query.OrderByDescending(t => t.Seller!.UserName) : query.OrderBy(t => t.Seller!.UserName),
            _ => query.OrderBy(t => t.Id)
        };
        return query;
    }

    public async Task<bool> IsPostInTransactionAsync(long postId)
    {
        return await context.Transactions.AnyAsync(t => t.PostId == postId);
    }
    
    public async Task<bool> IsPostInOnGoingTransactionAsync(long postId)
    {
        return await context.Transactions.AnyAsync(t => t.PostId == postId && !t.TransactionStatus!.IsFinal);
    }
}