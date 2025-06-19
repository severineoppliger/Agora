using Agora.Core.Enums;
using Agora.Core.Interfaces.Filters;
using Agora.Core.Models;

namespace Agora.Core.Interfaces.Repositories;

public interface ITransactionStatusRepository
{
    /// <summary>
    /// Retrieves all transaction status, optionally filtered by the provided filter.
    /// </summary>
    /// <param name="filter">An optional filter to apply to the transaction status query.</param>
    /// <returns>A list of <see cref="TransactionStatus"/> objects matching the filter.</returns>
    Task<IReadOnlyList<TransactionStatus>> GetAllTransactionStatusAsync(ITransactionStatusFilter filter);
    
    /// <summary>
    /// Retrieves a transaction status by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the transaction status.</param>
    /// <returns>
    /// The <see cref="TransactionStatus"/> if found; otherwise, <c>null</c>.
    /// </returns>
    Task<TransactionStatus?> GetTransactionStatusByIdAsync(long id);
    
    /// <summary>
    /// Retrieves a transaction status by its enum value.
    /// </summary>
    /// <param name="statusEnum">The enum value of the transaction status.</param>
    /// <returns>
    /// The <see cref="TransactionStatus"/> if found; otherwise, <c>null</c>.
    /// </returns>
    Task<TransactionStatus?> GetTransactionStatusByEnumAsync(TransactionStatusEnum statusEnum);
    
    /// <summary>
    /// Retrieves a transaction status identifier by its enum value.
    /// </summary>
    /// <param name="statusEnum">The enum value of the transaction status.</param>
    /// <returns>
    /// The transaction status identifier if found; otherwise it throws an error.
    /// </returns>
    Task<long> GetIdByEnumAsync(TransactionStatusEnum statusEnum);
    
    /// <summary>
    /// Persists all pending changes in the repository to the database.
    /// </summary>
    /// <returns>
    /// <c>true</c> if the changes were saved successfully; otherwise, <c>false</c>.
    /// </returns>
    Task<bool> SaveChangesAsync();
    
    /// <summary>
    /// Checks whether a transaction status with the specified name exists.
    /// </summary>
    /// <param name="name">The name of the transaction status.</param>
    /// <returns>
    /// <c>true</c> if a transaction status with the given name exists; otherwise, <c>false</c>.
    /// </returns>
    Task<bool> NameExistsAsync(string name);
}