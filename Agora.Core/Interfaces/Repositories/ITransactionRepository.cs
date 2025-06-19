using Agora.Core.Interfaces.QueryParameters;
using Agora.Core.Models;
using Agora.Core.Models.Entities;

namespace Agora.Core.Interfaces.Repositories;

public interface ITransactionRepository
{
    /// <summary>
    /// Retrieves all transactions, optionally filtered by the provided queryParameters.
    /// </summary>
    /// <param name="queryParameters">An optional queryParameters to apply to the transaction query.</param>
    /// <returns>A list of <see cref="Transaction"/> objects matching the queryParameters.</returns>
    Task<IReadOnlyList<Transaction>> GetAllTransactionsAsync(ITransactionQueryParameters queryParameters);
    
    /// <summary>
    /// Retrieves a transaction by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the transaction.</param>
    /// <returns>
    /// The <see cref="Transaction"/> if found; otherwise, <c>null</c>.
    /// </returns>
    Task<Transaction?> GetTransactionByIdAsync(long id);
    
    /// <summary>
    /// Adds a new transaction to the repository. 
    /// Changes must be saved using <see cref="SaveChangesAsync"/>.
    /// </summary>
    /// <param name="transaction">The transaction to add.</param>
    void AddTransaction(Transaction transaction);
    
    /// <summary>
    /// Persists all pending changes in the repository to the database.
    /// </summary>
    /// <returns>
    /// <c>true</c> if the changes were saved successfully; otherwise, <c>false</c>.
    /// </returns>
    Task<bool> SaveChangesAsync();
    
    /// <summary>
    /// Checks whether a specific <c>Post</c> is linked to any transaction (in any status).
    /// </summary>
    /// <param name="postId">The unique identifier of the post.</param>
    /// <returns>
    /// <c>true</c> if the specified <c>Post</c> is linked to any transaction; otherwise, <c>false</c>.
    /// </returns>
    Task<bool> IsPostInTransactionAsync(long postId);
    
    /// <summary>
    /// Checks whether a specific <c>Post</c> is linked to any ongoing transaction.
    /// </summary>
    /// <param name="postId">The unique identifier of the post.</param>
    /// <returns>
    /// <c>true</c> if the specified <c>Post</c> is linked to any ongoing transaction; otherwise, <c>false</c>.
    /// </returns>
    public Task<bool> IsPostInOnGoingTransactionAsync(long postId);
}