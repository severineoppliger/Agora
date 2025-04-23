using Agora.Core.Interfaces;
using Agora.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace Agora.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionsController(ITransactionRepository repo) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Transaction>>> GetAllTransactions()
    {
        return Ok(await repo.GetAllTransactionsAsync());
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<Transaction>> GetTransaction([FromRoute] long id)
    {
        Transaction? transaction = await repo.GetTransactionByIdAsync(id);

        if (transaction == null) return NotFound();

        return Ok(transaction);
    }
    
    [HttpPost]
    public async Task<ActionResult<Transaction>> CreateTransaction([FromBody] Transaction transaction)
    {
        repo.AddTransaction(transaction);
        
        if (await repo.SaveChangesAsync())
        {
            return CreatedAtAction("GetTransaction", new { id = transaction.Id }, transaction);
        }
        
        return BadRequest("Problem creating the transaction.");
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult> UpdateTransaction([FromRoute] long id, [FromBody] Transaction transaction)
    {
        if (transaction.Id != id || !TransactionExists(id))
        {
            return BadRequest("Transaction doesn't exist or there is an incoherence between route and body ids.");
        }
        
        repo.UpdateTransaction(transaction);

        if (await repo.SaveChangesAsync())
        {
            return NoContent();
        }

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

    private bool TransactionExists(long id)
    {
        return repo.TransactionExists(id);
    }
}