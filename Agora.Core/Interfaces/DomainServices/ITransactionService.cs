﻿using Agora.Core.Commands;
using Agora.Core.Enums;
using Agora.Core.Models;
using Agora.Core.Models.DomainQueryParameters;
using Agora.Core.Models.Entities;
using Agora.Core.Shared;

namespace Agora.Core.Interfaces.DomainServices;

/// <summary>
/// Encapsulates business rules for handling transactions between users,
/// including creation, updates, and validation of transaction flows.
/// </summary>
public interface ITransactionService
{
    /// <summary>
    /// Retrieves all <c>Transaction</c> visible to the user based on authorization rules and visibility mode.
    /// </summary>
    /// <param name="transactionVisibilityMode">Specifies the scope of transactions to retrieve
    /// (e.g., current user's transactions or admin view).</param>
    /// <param name="queryParams">Filter criteria to apply when querying transactions.</param>
    /// <param name="userContext">Context of the current user making the request.</param>
    /// <returns>A successful Result wrapping a list of <c>Transaction</c> visible to user,
    /// or failure if an error occurs.</returns>
    public Task<Result<IReadOnlyList<Transaction>>> GetAllTransactionsAsync(
        TransactionVisibilityMode transactionVisibilityMode,
        TransactionQueryParameters queryParams,
        UserContext userContext
    );
    
    /// <summary>
    /// Retrieves a single <c>Transaction</c> by its ID if visible to the user.
    /// </summary>
    /// <param name="transactionId">The ID of the <c>Transaction</c> to retrieve.</param>
    /// <param name="userContext">Context of the current user requesting the <c>Transaction</c>.</param>
    /// <returns>
    /// Success wrapping the <c>Transaction</c> if found and authorized,
    /// failure with NotFound if missing,
    /// or failure with Unauthorized if the user cannot view it.
    /// </returns>
    public Task<Result<Transaction>> GetTransactionByIdAsync(long transactionId, UserContext userContext);
    
    /// <summary>
    /// Creates a new <c>Transaction</c> after validating authorization and business rules.
    /// </summary>
    /// <param name="transaction">The <c>Transaction</c> entity to create.</param>
    /// <param name="userContext">Context of the user creating the <c>Transaction</c>.</param>
    /// <returns>
    /// Success wrapping the created <c>Transaction</c> if successful,
    /// or failure with relevant error details.
    /// </returns>
    public Task<Result<Transaction>> CreateTransactionAsync(
        Transaction transaction,
        UserContext userContext);
    
    /// <summary>
    /// Updates specified details of an existing <c>Transaction</c> after validating authorization and business rules.
    /// Only non-null values in <paramref name="newDetails"/> are applied.
    /// </summary>
    /// <param name="transactionId">ID of the <c>Transaction</c> to update.</param>
    /// <param name="newDetails">The details to update.</param>
    /// <param name="userContext">Context of the user performing the update.</param>
    /// <returns>
    /// Success if update and save are successful,
    /// or failure with appropriate error messages.
    /// </returns>
    public Task<Result> UpdateTransactionDetailsAsync(
        long transactionId,
        UpdateTransactionDetailsCommand newDetails,
        UserContext userContext);
    
    /// <summary>
    /// Changes the status of an existing <c>Transaction</c> after authorization and validation of state transitions.
    /// </summary>
    /// <param name="transactionId">ID of the <c>Transaction</c> to update status for.</param>
    /// <param name="userContext">Context of the user performing the status change.</param>
    /// <param name="newStatus">The new <c>Transaction</c> status to apply.</param>
    /// <returns>
    /// Success if the status was changed and saved successfully,
    /// or failure with relevant error details if invalid or unauthorized.
    /// </returns>
    public Task<Result> ChangeTransactionStatusAsync(
        long transactionId,
        UserContext userContext,
        TransactionStatusEnum newStatus);
}