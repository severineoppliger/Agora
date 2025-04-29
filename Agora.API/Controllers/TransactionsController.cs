using Agora.API.DTOs.Transaction;
using Agora.Core.Interfaces;
using Agora.Core.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Agora.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionsController(ITransactionRepository repo, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<TransactionSummaryDto>>> GetAllTransactions()
    {
        IReadOnlyList<Transaction> transactions = await repo.GetAllTransactionsAsync();
        return Ok(mapper.Map<IReadOnlyList<TransactionSummaryDto>>(transactions));
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<TransactionDetailsDto>> GetTransaction([FromRoute] long id)
    {
        Transaction? transaction = await repo.GetTransactionByIdAsync(id);

        if (transaction == null) return NotFound();

        return Ok(mapper.Map<TransactionDetailsDto>(transaction));
    }
    
    [HttpPost]
    public async Task<ActionResult<TransactionDetailsDto>> CreateTransaction([FromBody] CreateTransactionDto transactionDto)
    {
        Transaction transaction = mapper.Map<Transaction>(transactionDto);
        repo.AddTransaction(transaction);
        
        if (await repo.SaveChangesAsync())
        {
            Transaction? createdTransaction = await repo.GetTransactionByIdAsync(transaction.Id);
            
            if (createdTransaction == null)
            {
                return StatusCode(500, "Transaction was saved but could not be retrieved.");
            }
            
            TransactionDetailsDto createdTransactionDetailsDto = mapper.Map<TransactionDetailsDto>(createdTransaction);
            
            return CreatedAtAction(nameof(GetTransaction), new { id = createdTransactionEntity.Id }, createdTransactionDetailsDto);
        }
        
        return BadRequest("Problem creating the transaction.");
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult> UpdateTransaction([FromRoute] long id, [FromBody] UpdateTransactionDto transactionDto)
    {
        Transaction? existingTransaction = await repo.GetTransactionByIdAsync(id);

        if (existingTransaction == null) return NotFound();
        
        // Apply the updated fields exposed in the DTO to the existing transaction
        mapper.Map(transactionDto, existingTransaction);

        if (await repo.SaveChangesAsync()) return NoContent();

        return BadRequest("Problem updating the transaction.");
    }

    [HttpDelete("{id:long}")]
    public async Task<ActionResult> DeleteTransaction([FromRoute] long id)
    {
        Transaction? transaction = await repo.GetTransactionByIdAsync(id);

        if (transaction == null)
        {
            return NotFound();
        }

        repo.DeleteTransaction(transaction);

        if (await repo.SaveChangesAsync())
        {
            return NoContent();
        }

        return BadRequest("Problem deleting the transaction.");
    }
}