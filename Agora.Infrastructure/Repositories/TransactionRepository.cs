using Agora.Core.Interfaces;
using Agora.Core.Models;
using Agora.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Agora.Infrastructure.Repositories;

public class TransactionRepository(AgoraDbContext context) : ITransactionRepository
{
    public async Task<IReadOnlyList<Transaction>> GetAllTransactionsAsync(ITransactionFilter filter, string? userId = null)
    {
        IQueryable<Transaction> transactions = context.Transactions.AsQueryable();

        // A not null userId means the user is not an admin and can only access its own transactions
        if (!string.IsNullOrEmpty(userId))
        {
            transactions = transactions.Where(t => t.BuyerId == userId || t.SellerId == userId);
        }

        if (filter.MinPrice.HasValue)
        {
            transactions = transactions.Where(t => t.Price >= filter.MinPrice);
        }

        if (filter.MaxPrice.HasValue)
        {
            transactions = transactions.Where(t => t.Price <= filter.MaxPrice);
        }

        if (!string.IsNullOrWhiteSpace(filter.PostTitle))
        {
            transactions = transactions.Where(t => t.Post != null && t.Post.Title.Contains(filter.PostTitle));
        }

        if (!string.IsNullOrWhiteSpace(filter.TransactionStatusName))
        {
            transactions = transactions.Where(t => t.TransactionStatus != null && t.TransactionStatus.Name.Contains(filter.TransactionStatusName));
        }

        if (!string.IsNullOrWhiteSpace(filter.UsersInvolvedUsername))
        {
            transactions = transactions.Where(t => t.Buyer != null && t.Buyer.UserName!.Contains(filter.UsersInvolvedUsername) || t.Seller != null && t.Seller.UserName!.Contains(filter.UsersInvolvedUsername));
        }
        
        transactions = ApplySorting(transactions, filter);
        
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

    public IQueryable<Transaction> ApplySorting(IQueryable<Transaction> query, ITransactionFilter queryParams)
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
}