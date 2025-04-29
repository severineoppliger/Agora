using Agora.API.DTOs.Transaction;
using Agora.API.InputValidation.Interfaces;
using Agora.API.Orchestrators.Interfaces;
using Agora.Core.Interfaces;
using Agora.Core.Models;
using AutoMapper;
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
    
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<TransactionSummaryDto>>> GetAllTransactions()
    {
        IReadOnlyList<Transaction> transactions = await repo.GetAllTransactionsAsync();
        return Ok(mapper.Map<IReadOnlyList<TransactionSummaryDto>>(transactions));
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<TransactionDetailsDto>> GetTransaction([FromRoute] long id)
    {
        Transaction? transactionEntity = await repo.GetTransactionByIdAsync(id);

        if (transactionEntity == null) return NotFound(TransactionNotFoundMessage);

        return Ok(mapper.Map<TransactionDetailsDto>(transactionEntity));
    }
    
    [HttpPost]
    public async Task<ActionResult<TransactionDetailsDto>> CreateTransaction([FromBody] CreateTransactionDto transactionDto)
    {
        // Input validation
        List<string> inputErrors = await inputValidator.ValidateCreateTransactionDtoAsync(transactionDto);
        if (inputErrors.Count != 0)
            return BadRequest(new { Errors = inputErrors });
        
        // Transform to the full entity and validate with business rules
        Transaction transactionEntity = mapper.Map<Transaction>(transactionDto);
        IList<string> businessRulesErrors = await businessRulesValidationOrchestrator.ValidateAndProcessTransactionAsync(transactionEntity);
        if (businessRulesErrors.Count != 0)
            return BadRequest(new { Errors = businessRulesErrors });
        
        // Add to database
        repo.AddTransaction(transactionEntity);
        
        if (await repo.SaveChangesAsync())
        {
            Transaction? createdTransactionEntity = await repo.GetTransactionByIdAsync(transactionEntity.Id);
            
            if (createdTransactionEntity == null)
            {
                return StatusCode(500, "Transaction was saved but could not be retrieved.");
            }
            
            TransactionDetailsDto createdTransactionDetailsDto = mapper.Map<TransactionDetailsDto>(createdTransactionEntity);
            
            return CreatedAtAction(nameof(GetTransaction), new { id = createdTransactionEntity.Id }, createdTransactionDetailsDto);
        }
        
        return BadRequest("Problem creating the transaction.");
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult> UpdateTransaction([FromRoute] long id, [FromBody] UpdateTransactionDto transactionDto)
    {
        Transaction? existingTransactionEntity = await repo.GetTransactionByIdAsync(id);

        if (existingTransactionEntity == null) return NotFound(TransactionNotFoundMessage);
        
        // Apply the updated fields exposed in the DTO to the existing transaction
        mapper.Map(transactionDto, existingTransactionEntity);

        return await repo.SaveChangesAsync() ? NoContent() : BadRequest("Problem updating the transaction.");
    }

    [HttpDelete("{id:long}")]
    public async Task<ActionResult> DeleteTransaction([FromRoute] long id)
    {
        Transaction? transaction = await repo.GetTransactionByIdAsync(id);

        if (transaction == null)
        {
            return NotFound(TransactionNotFoundMessage);
        }

        repo.DeleteTransaction(transaction);

        return await repo.SaveChangesAsync() ? NoContent() : BadRequest("Problem deleting the transaction.");
    }
}