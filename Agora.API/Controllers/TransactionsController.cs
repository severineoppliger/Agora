using System.Security.Claims;
using Agora.API.DTOs.Transaction;
using Agora.API.InputValidation.Interfaces;
using Agora.API.Orchestrators.Interfaces;
using Agora.API.QueryParams;
using Agora.Core.Constants;
using Agora.Core.Interfaces;
using Agora.Core.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Agora.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionsController(
    ITransactionRepository repo,
    IMapper mapper,
    IInputValidator inputValidator,
    IBusinessRulesValidationOrchestrator businessRulesValidationOrchestrator)
    : ControllerBase
{
    private const string TransactionNotFoundMessage = "Transaction not found.";
    private const string NotInvolvedMessage = "Current user is not involved in the transaction.";
    
    // An admin has access to all transactions, but normal user have only access to transactions in which it is involved.
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<TransactionSummaryDto>>> GetAllTransactions(
        [FromQuery] TransactionQueryParameters queryParameters)
    {
        string? currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (currentUserId != null)
        {
            return Unauthorized("User ID not found in claims.");
        }
        
        bool isAdmin = User.IsInRole(Roles.Admin);
        
        IReadOnlyList<Transaction> transactions = await repo.GetAllTransactionsAsync(queryParameters, isAdmin ? null : currentUserId);
        return Ok(mapper.Map<IReadOnlyList<TransactionSummaryDto>>(transactions));
    }

    // The current user should either be an admin or be involved in the transaction to see it
    [Authorize]
    [HttpGet("{id:long}")]
    public async Task<ActionResult<TransactionDetailsDto>> GetTransaction([FromRoute] long id)
    {
        Transaction? transaction = await repo.GetTransactionByIdAsync(id);

        if (transaction == null)
        {
            return NotFound(TransactionNotFoundMessage);
        }
        
        // Ownership validation
        string? currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (transaction.BuyerId != currentUserId && transaction.SellerId != currentUserId)
        {
            return Unauthorized(NotInvolvedMessage);
        }
        
        return Ok(mapper.Map<TransactionDetailsDto>(transaction));
    }
    
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<TransactionDetailsDto>> CreateTransaction([FromBody] CreateTransactionDto transactionDto)
    {
        // Input validation
        List<string> inputErrors = await inputValidator.ValidateInputTransactionDtoAsync(transactionDto);
        if (inputErrors.Count != 0)
        {
            return BadRequest(new { Errors = inputErrors });
        }
        
        // Transform to the full entity and validate with business rules
        Transaction transaction = mapper.Map<Transaction>(transactionDto);
        IList<string> businessRulesErrors = await businessRulesValidationOrchestrator.ValidateAndProcessTransactionAsync(transaction);
        if (businessRulesErrors.Count != 0)
        {
            return BadRequest(new { Errors = businessRulesErrors });
        }
        
        // Add to database
        repo.AddTransaction(transaction);
        
        if (await repo.SaveChangesAsync())
        {
            Transaction? createdTransaction = await repo.GetTransactionByIdAsync(transaction.Id);
            
            if (createdTransaction == null)
            {
                return StatusCode(500, "Transaction was saved but could not be retrieved.");
            }
            
            TransactionDetailsDto createdTransactionDetailsDto = mapper.Map<TransactionDetailsDto>(createdTransaction);
            
            return CreatedAtAction(nameof(GetTransaction), new { id = createdTransaction.Id }, createdTransactionDetailsDto);
        }
        
        return BadRequest("Problem creating the transaction.");
    }

    [Authorize]
    [HttpPut("{id:long}")]
    public async Task<ActionResult> UpdateTransaction([FromRoute] long id, [FromBody] UpdateTransactionDto transactionDto)
    {
        // Retrieve the existing transaction
        Transaction? existingTransaction = await repo.GetTransactionByIdAsync(id);
        if (existingTransaction == null)
        {
            return NotFound(TransactionNotFoundMessage);
        }
        
        // Ownership validation
        string? currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (existingTransaction.BuyerId != currentUserId && existingTransaction.SellerId != currentUserId)
        {
            return Unauthorized(NotInvolvedMessage);
        }
        
        // Input validation
        List<string> inputErrors = await inputValidator.ValidateInputTransactionDtoAsync(transactionDto);
        if (inputErrors.Count != 0)
        {
            return BadRequest(new { Errors = inputErrors });
        }
        
        // Transform to the full entity and validate with business rules
        Transaction transaction = mapper.Map<Transaction>(transactionDto);
        IList<string> businessRulesErrors = await businessRulesValidationOrchestrator.ValidateAndProcessTransactionAsync(transaction);
        if (businessRulesErrors.Count != 0)
        {
            return BadRequest(new { Errors = businessRulesErrors });
        }
        
        // Apply the updated fields exposed in the DTO to the existing transaction
        mapper.Map(transactionDto, existingTransaction);

        return await repo.SaveChangesAsync()
            ? NoContent()
            : BadRequest("Problem updating the transaction.");
    }

    [Authorize]
    [HttpDelete("{id:long}")]
    public async Task<ActionResult> DeleteTransaction([FromRoute] long id)
    {
        Transaction? transaction = await repo.GetTransactionByIdAsync(id);

        if (transaction == null)
        {
            return NotFound(TransactionNotFoundMessage);
        }
        
        // Ownership validation
        string? currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (transaction.BuyerId != currentUserId && transaction.SellerId != currentUserId)
        {
            return Unauthorized(NotInvolvedMessage);
        }

        repo.DeleteTransaction(transaction);

        return await repo.SaveChangesAsync()
            ? NoContent()
            : BadRequest("Problem deleting the transaction.");
    }
}