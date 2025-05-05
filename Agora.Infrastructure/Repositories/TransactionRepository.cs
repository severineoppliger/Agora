using Agora.Core.Interfaces;
using Agora.Core.Models;
using Agora.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Agora.Infrastructure.Repositories;

public class TransactionRepository(AgoraDbContext context) : ITransactionRepository
{
    public async Task<IReadOnlyList<Transaction>> GetAllTransactionsAsync(ITransactionFilter filter)
    {
        IQueryable<Transaction> transactions = context.Transactions.AsQueryable();

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
            transactions = transactions.Where(t => t.Buyer != null && t.Buyer.Username.Contains(filter.UsersInvolvedUsername) || t.Seller != null && t.Seller.Username.Contains(filter.UsersInvolvedUsername));
        }
        
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
}