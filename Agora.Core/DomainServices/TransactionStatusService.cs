using Agora.Core.Commands;
using Agora.Core.Constants;
using Agora.Core.Enums;
using Agora.Core.Interfaces.DomainServices;
using Agora.Core.Interfaces.QueryParameters;
using Agora.Core.Interfaces.Repositories;
using Agora.Core.Models.Entities;
using Agora.Core.Shared;
using Agora.Core.Validation.Interfaces;

namespace Agora.Core.DomainServices;

/// <summary>
/// Default implementation of <see cref="ITransactionStatusService"/>.
/// </summary>
public class TransactionStatusService(
    ITransactionStatusRepository transactionStatusRepo,
    IBusinessRulesValidator businessRulesValidator
    ) : ITransactionStatusService
{
    private const string EntityName = "transaction status";
    
    /// <inheritdoc />
    public async Task<Result<IReadOnlyList<TransactionStatus>>> GetAllTransactionStatusAsync(ITransactionStatusQueryParameters queryParams)
    {
        // Validate business rules
        Result businessRulesValidationResult = businessRulesValidator.ValidateSortBy(queryParams.SortBy, SortByOptions.TransactionStatus);
        if (businessRulesValidationResult.IsFailure)
        {
            return Result<IReadOnlyList<TransactionStatus>>.Failure(businessRulesValidationResult.Errors!);
        }
        
        // Retrieve in database
        IReadOnlyList<TransactionStatus> transactionStatus = await transactionStatusRepo.GetAllTransactionStatusAsync(queryParams);
        
        return Result<IReadOnlyList<TransactionStatus>>.Success(transactionStatus);
    }

    /// <inheritdoc />
    public async Task<Result<TransactionStatus>> GetTransactionStatusByIdAsync(long transactionStatusId)
    {
        TransactionStatus? transactionStatus =
            await transactionStatusRepo.GetTransactionStatusByIdAsync(transactionStatusId);
        
        return transactionStatus is null
            ? Result<TransactionStatus>.Failure(ErrorType.NotFound, ErrorMessages.NotFound(EntityName))
            : Result<TransactionStatus>.Success(transactionStatus);
    }
    
    /// <inheritdoc />
    public async Task<Result> UpdateTransactionStatusDetailsAsync(long transactionStatusId, UpdateTransactionStatusDetailsCommand newDetails)
    {
        // Retrieve the existing transaction status
        TransactionStatus? transactionStatus = await transactionStatusRepo.GetTransactionStatusByIdAsync(transactionStatusId);
        if (transactionStatus == null)
        {
            return Result.Failure(ErrorType.NotFound,ErrorMessages.NotFound(EntityName));
        }
        
        // Validate business rules
        Result businessRulesValidationResult =
            await businessRulesValidator.ValidateTransactionStatusUpdateAsync(transactionStatus, newDetails);
        if (businessRulesValidationResult.IsFailure)
        {
            return businessRulesValidationResult;
        }

        // Apply modifications
        if (newDetails.Name is not null)
        {
            transactionStatus.Name = newDetails.Name;
        }
        
        if (newDetails.Description is not null)
        {
            transactionStatus.Description = newDetails.Description;
        }

        // Save updates
        return await transactionStatusRepo.SaveChangesAsync()
            ? Result.Success()
            : Result.Failure(ErrorType.Persistence,ErrorMessages.ErrorWhenSavingToDb(EntityName));
    }
}