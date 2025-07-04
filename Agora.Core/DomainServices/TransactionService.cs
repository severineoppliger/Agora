﻿using Agora.Core.Commands;
using Agora.Core.Constants;
using Agora.Core.Enums;
using Agora.Core.Interfaces.DomainServices;
using Agora.Core.Interfaces.Repositories;
using Agora.Core.Models;
using Agora.Core.Models.DomainQueryParameters;
using Agora.Core.Models.Entities;
using Agora.Core.Shared;
using Agora.Core.Validation.Interfaces;

namespace Agora.Core.DomainServices;

/// <summary>
/// Default implementation of <see cref="ITransactionService"/>.
/// </summary>
public class TransactionService(
    IPostRepository postRepo,
    ITransactionRepository transactionRepo,
    ITransactionStatusRepository transactionStatusRepo,
    IUserRepository userRepo,
    IUserService userService,
    IAuthorizationValidator authorizationValidator,
    IBusinessRulesValidator businessRulesValidator
    ) : ITransactionService
{
    private const string EntityName = "transaction";
    
    /// <inheritdoc />
    public async Task<Result<IReadOnlyList<Transaction>>> GetAllTransactionsAsync(
        TransactionVisibilityMode transactionVisibilityMode,
        TransactionQueryParameters queryParams,
        UserContext userContext
    )
    {
        // Validate business rules
        Result businessRulesValidationResult = businessRulesValidator.ValidateSortBy(queryParams.SortBy, SortByOptions.Transaction);
        if (businessRulesValidationResult.IsFailure)
        {
            return Result<IReadOnlyList<Transaction>>.Failure(businessRulesValidationResult.Errors!);
        }
        
        // Enhance queryParameters according to business rules
        switch (transactionVisibilityMode)
        {
            case TransactionVisibilityMode.CurrentUserTransactions:
                queryParams.UserInvolvedId = userContext.UserId;
                break;
            case TransactionVisibilityMode.AdminView:
                if (!userContext.IsAdmin)
                {
                    return Result<IReadOnlyList<Transaction>>.Failure(ErrorType.Unauthorized,
                        ErrorMessages.User.NotAuthorized);
                }

                break;
            default:
                return Result<IReadOnlyList<Transaction>>.Failure(ErrorType.Invalid,
                    ErrorMessages.IsInvalid("transaction visibility mode", transactionVisibilityMode.ToString()));
        }
        
        // Get filtered transactions from database
        IReadOnlyList<Transaction> transactions = await transactionRepo.GetAllTransactionsAsync(queryParams);
        
        return Result<IReadOnlyList<Transaction>>.Success(transactions);
    }

    /// <inheritdoc />
    public async Task<Result<Transaction>> GetTransactionByIdAsync(long transactionId, UserContext userContext)
    {
        Transaction? transaction = await transactionRepo.GetTransactionByIdAsync(transactionId);
        if (transaction is null)
        {
            return Result<Transaction>.Failure(ErrorType.NotFound,ErrorMessages.NotFound(EntityName));
        }
        
        return !authorizationValidator.CanViewTransaction(transaction, userContext)
            ? Result<Transaction>.Failure(ErrorType.Forbidden,ErrorMessages.Transaction.NotInvolved)
            : Result<Transaction>.Success(transaction);
    }
    
    /// <inheritdoc />
    public async Task<Result<Transaction>> CreateTransactionAsync(Transaction transaction, UserContext userContext)
    {
        // Control authorization to create the transaction
        if (!authorizationValidator.CanCreateTransaction(transaction, userContext))
        {
            return Result<Transaction>.Failure(ErrorType.Forbidden,ErrorMessages.User.NotAuthorized);
        }
        
        // Complete transaction information
        transaction.InitiatorId = userContext.UserId;
        transaction.CreatedAt = DateTime.UtcNow;
        transaction.TransactionStatusId = await transactionStatusRepo.GetIdByEnumAsync(TransactionStatusEnum.Pending);
        
        // Validate business rules (need navigation properties)
        Result<Transaction> enhanceTransactionResult = await EnhanceTransactionWithNavigationProperties(transaction);
        if (enhanceTransactionResult.IsFailure)
        {
            return enhanceTransactionResult;
        }
        Transaction? enhancedTransaction = enhanceTransactionResult.Value;
        if (enhancedTransaction is null)
        {
            return Result<Transaction>.Failure(ErrorType.Unknown,
                ErrorMessages.UnknownErrorDuringAction("transaction", "enhancement with navigation properties"));
        }
        
        Result businessRulesValidationResult = businessRulesValidator.ValidateNewTransaction(enhancedTransaction);
        if (businessRulesValidationResult.IsFailure)
        {
            return Result<Transaction>.Failure(businessRulesValidationResult.Errors!);
        }
        
        // Add to database
        transactionRepo.AddTransaction(transaction);
        
        if (await transactionRepo.SaveChangesAsync())
        {
            Transaction? createdTransaction = await transactionRepo.GetTransactionByIdAsync(transaction.Id);

            return createdTransaction == null 
                ? Result<Transaction>.Failure(ErrorType.Persistence, ErrorMessages.SavedButNotRetrieved(EntityName))
                : Result<Transaction>.Success(createdTransaction);
        }

        return Result<Transaction>.Failure(ErrorType.Persistence,ErrorMessages.ErrorWhenSavingToDb(EntityName));
    }

    /// <inheritdoc />
    public async Task<Result> UpdateTransactionDetailsAsync(
        long transactionId,
        UpdateTransactionDetailsCommand newDetails,
        UserContext userContext)
    {
        // Retrieve the existing transaction
        Transaction? transaction = await transactionRepo.GetTransactionByIdAsync(transactionId);
        if (transaction == null)
        {
            return Result.Failure(ErrorType.NotFound,ErrorMessages.NotFound(EntityName));
        }
        
        // Control authorization to modify the transaction
        if (!authorizationValidator.CanManageTransaction(transaction, userContext))
        {
            return Result.Failure(ErrorType.Forbidden,ErrorMessages.User.NotAuthorized);
        }
        
        // Apply modifications
        if (newDetails.Title is not null)
        {
            transaction.Title = newDetails.Title;
        }

        if (newDetails.Price.HasValue)
        {
            transaction.Price = newDetails.Price.Value;
        }

        if (newDetails.PostId.HasValue)
        {
            transaction.PostId = newDetails.PostId.Value;
        }

        if (newDetails.TransactionDate.HasValue)
        {
            transaction.TransactionDate = newDetails.TransactionDate.Value;
        }
        
        transaction.UpdatedAt = DateTime.UtcNow;
        
        // Validate business rules of transaction
        Result businessRulesValidationResult = businessRulesValidator.ValidateTransactionUpdate(transaction);
        if (businessRulesValidationResult.IsFailure)
        {
            return businessRulesValidationResult;
        }
        
        // Save updates
        return await transactionRepo.SaveChangesAsync()
            ? Result.Success()
            : Result.Failure(ErrorType.Persistence,ErrorMessages.ErrorWhenSavingToDb(EntityName));
    }
    
    /// <inheritdoc />
    public async Task<Result> ChangeTransactionStatusAsync(
        long transactionId,
        UserContext userContext,
        TransactionStatusEnum newStatus)
    {
        // Retrieve the existing transaction and necessary information
        Transaction? transaction = await transactionRepo.GetTransactionByIdAsync(transactionId);
        if (transaction == null)
        {
            return Result.Failure(ErrorType.NotFound,ErrorMessages.NotFound(EntityName));
        }
        
        // Control authorization to modify the transaction
        if (!authorizationValidator.CanManageTransaction(transaction, userContext))
        {
            return Result.Failure(ErrorType.Forbidden,ErrorMessages.User.NotAuthorized);
        }
        
        // Validate business rules
        TransactionStatusEnum oldStatus = transaction.TransactionStatus!.EnumValue;
        Result statusChangeValidationResult =
            businessRulesValidator.ValidateTransactionStatusChange(transaction, oldStatus, newStatus, userContext);
        if (statusChangeValidationResult.IsFailure)
        {
            return statusChangeValidationResult;
        }
        
        // Business logic according to the origin transaction status
        switch (oldStatus)
        {
            case TransactionStatusEnum.Pending or TransactionStatusEnum.InDispute:
                break;
            case TransactionStatusEnum.Accepted:
                if (newStatus == TransactionStatusEnum.PartiallyValidated)
                    if (userContext.UserId == transaction.BuyerId)
                    {
                        transaction.BuyerConfirmed = true;
                    } else if (userContext.UserId == transaction.SellerId)
                    {
                        transaction.SellerConfirmed = true;
                    }
                break;
            case TransactionStatusEnum.PartiallyValidated:
                if (newStatus == TransactionStatusEnum.Completed)
                    if (userContext.UserId == transaction.BuyerId)
                    {
                        transaction.BuyerConfirmed = true;
                    } else if (userContext.UserId == transaction.SellerId)
                    {
                        transaction.SellerConfirmed = true;
                    }
                break;
            default:
                return Result.Failure(ErrorType.Unknown, ErrorMessages.UnknownErrorDuringAction("transaction status", "change"));
        }

        transaction.TransactionStatusId = await transactionStatusRepo.GetIdByEnumAsync(newStatus);
        
        // Business rules to transfer Credit or not
        TransactionStatus? oldTransactionStatus = await transactionStatusRepo.GetTransactionStatusByEnumAsync(oldStatus);
        if (oldTransactionStatus is null)
        {
            return Result.Failure(ErrorType.NotFound,
                ErrorMessages.NotFound("transaction status", oldStatus.ToString()));
        }
        
        TransactionStatus? newTransactionStatus = await transactionStatusRepo.GetTransactionStatusByEnumAsync(newStatus);
        if (newTransactionStatus is null)
        {
            return Result.Failure(ErrorType.NotFound,
                ErrorMessages.NotFound("transaction status", newStatus.ToString()));
        }

        bool mustTransferCredit = !oldTransactionStatus.IsSuccess 
                                  && newTransactionStatus is { IsFinal: true, IsSuccess: true };

        if (mustTransferCredit)
        {
            Result transferResult = await PerformCreditTransfer(transaction);
            if (transferResult.IsFailure)
            {
                return transferResult;
            }
        }
        
        // Complete information
        bool completedTransaction = !oldTransactionStatus.IsFinal && newTransactionStatus.IsFinal;
        if (completedTransaction)
        {
            transaction.CompletedAt = DateTime.UtcNow;
        }
        
        return await transactionRepo.SaveChangesAsync()
            ? Result.Success()
            : Result.Failure(ErrorType.Persistence,ErrorMessages.ErrorWhenSavingToDb(EntityName));
    }


    /// <summary>
    /// Attach the necessary navigation properties, for e.g. to validate their properties.
    /// </summary>
    private async Task<Result<Transaction>> EnhanceTransactionWithNavigationProperties(Transaction transaction)
    {
        transaction.Buyer = await userRepo.GetUserByIdAsync(transaction.BuyerId);
        if (transaction.Buyer is null)
        {
            return Result<Transaction>.Failure(ErrorType.NotFound, ErrorMessages.User.BuyerOrSellerDoesNotExist("buyer", transaction.BuyerId));
        }
              
        if (transaction.PostId != null)
        {
            transaction.Post = await postRepo.GetPostByIdAsync(transaction.PostId.Value);
            if (transaction.Post is null)
            {
                return Result<Transaction>.Failure(ErrorType.NotFound, ErrorMessages.RelatedEntityDoesNotExist("post", transaction.PostId));
            }
        }
        
        transaction.TransactionStatus = await transactionStatusRepo.GetTransactionStatusByIdAsync(transaction.TransactionStatusId);
        return transaction.TransactionStatus is null
            ? Result<Transaction>.Failure(ErrorType.NotFound, ErrorMessages.RelatedEntityDoesNotExist("transaction status", transaction.TransactionStatusId))
            : Result<Transaction>.Success(transaction);
    }
    
    private async Task<Result> PerformCreditTransfer(Transaction transaction)
    {
        User buyer = transaction.Buyer!;
        User seller = transaction.Seller!;
        int price = transaction.Price;

        if (!authorizationValidator.CanPayPrice(buyer, price))
        {
            return Result.Failure(ErrorType.Invalid, ErrorMessages.Transaction.CreditInsufficient);
        }

        return await userService.TransferCreditAsync(buyer, seller, price);
    }
}