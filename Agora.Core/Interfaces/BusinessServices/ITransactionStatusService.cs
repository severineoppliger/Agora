using Agora.Core.Common;
using Agora.Core.Interfaces.Filters;
using Agora.Core.Models;
using Agora.Core.Models.Requests;

namespace Agora.Core.Interfaces.BusinessServices;

public interface ITransactionStatusService
{
    /// <summary>
    /// Retrieves all transaction status, possibly filtered and sorted.
    /// </summary>
    /// <param name="transactionStatusFilter">Filter criteria to apply on transaction status.</param>
    /// <returns>A successful Result wrapping a list of transaction status, or failure if an error occurs.</returns>
    public Task<Result<IReadOnlyList<TransactionStatus>>> GetAllTransactionStatusAsync(ITransactionStatusFilter transactionStatusFilter);
    
    /// <summary>
    /// Retrieves a single transaction status by its ID.
    /// </summary>
    /// <param name="transactionStatusId">The ID of the transaction status to retrieve.</param>
    /// <returns>
    /// Success wrapping the transaction status if found,
    /// failure with NotFound if missing.
    /// </returns>
    public Task<Result<TransactionStatus>> GetTransactionStatusByIdAsync(long transactionStatusId);
    
    /// <summary>
    /// Updates specified details of an existing transaction status after validating authorization and business rules.
    /// Only non-null values in <paramref name="newDetails"/> are applied.
    /// </summary>
    /// <param name="transactionStatusId">ID of the transaction status to update.</param>
    /// <param name="newDetails">The details to update.</param>
    /// <param name="userContext">Context of the user performing the update.</param>
    /// <returns>
    /// Success if update and save are successful,
    /// or failure with appropriate error messages.
    /// </returns>
    public Task<Result> UpdateTransactionStatusDetailsAsync(
        long transactionStatusId,
        TransactionStatusDetailsUpdate newDetails);
}