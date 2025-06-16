using Agora.Core.BusinessRules.Interfaces;
using Agora.Core.Common;
using Agora.Core.Enums;
using Agora.Core.Interfaces.BusinessServices;
using Agora.Core.Interfaces.Filters;
using Agora.Core.Interfaces.Repositories;
using Agora.Core.Models;
using Agora.Core.Models.Requests;

namespace Agora.Core.BusinessServices;

public class TransactionStatusService(
    ITransactionStatusRepository transactionStatusRepo,
    IBusinessRulesValidator businessRulesValidator
    ) : ITransactionStatusService
{
    private const string EntityName = "transaction status";
    
    public async Task<Result<IReadOnlyList<TransactionStatus>>> GetAllTransactionStatusAsync(ITransactionStatusFilter transactionStatusFilter)
    {
        IReadOnlyList<TransactionStatus> transactionStatus = await transactionStatusRepo.GetAllTransactionStatusAsync(transactionStatusFilter);
        
        return Result<IReadOnlyList<TransactionStatus>>.Success(transactionStatus);
    }

    public async Task<Result<TransactionStatus>> GetTransactionStatusByIdAsync(long transactionStatusId)
    {
        TransactionStatus? transactionStatus =
            await transactionStatusRepo.GetTransactionStatusByIdAsync(transactionStatusId);
        
        return transactionStatus is null
            ? Result<TransactionStatus>.Failure(ErrorType.NotFound, ErrorMessages.NotFound(EntityName))
            : Result<TransactionStatus>.Success(transactionStatus);
    }
    
    public async Task<Result> UpdateTransactionStatusDetailsAsync(long transactionStatusId, TransactionStatusDetailsUpdate newDetails)
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