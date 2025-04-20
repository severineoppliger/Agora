using Agora.Core.Interfaces;
using Agora.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace Agora.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionStatusController(ITransactionStatusRepository repo) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<TransactionStatus>>> GetAllTransactionStatus()
    {
        return Ok(await repo.GetAllTransactionStatusAsync());
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<TransactionStatus>> GetTransactionStatus([FromRoute] long id)
    {
        TransactionStatus? transactionStatus = await repo.GetTransactionStatusByIdAsync(id);
        
        if (transactionStatus == null) return NotFound();
        
        return Ok(transactionStatus);
    }

    [HttpPost]
    async Task<ActionResult<TransactionStatus>> CreateTransactionStatus([FromBody] TransactionStatus transactionStatus)
    {
        repo.AddTransactionStatus(transactionStatus);

        if (await repo.SaveChangesAsync())
        {
            return CreatedAtAction("GetTransactionStatus", new { id = transactionStatus.Id }, transactionStatus);
        }

        return BadRequest("Problem creating the transaction status.");
    } 
    
    [HttpPut("{id:long}")]
    public async Task<ActionResult> UpdateTransactionStatus([FromRoute] long id, [FromBody] TransactionStatus transactionStatus)
    {
        if (transactionStatus.Id != id || !TransactionStatusExists(id))
        {
            return BadRequest("Transaction status doesn't exist or there is an incoherence between route and body ids.");
        }
        
        repo.UpdateTransactionStatus(transactionStatus);

        if (await repo.SaveChangesAsync())
        {
            return NoContent();
        }

        return BadRequest("Problem updating the transaction status.");
    }

    [HttpDelete("{id:long}")]
    public async Task<ActionResult> DeleteTransactionStatus([FromRoute] long id)
    {
        TransactionStatus? transactionStatus = await repo.GetTransactionStatusByIdAsync(id);

        if (transactionStatus == null)
        {
            return NotFound();
        }

        repo.DeleteTransactionStatus(transactionStatus);

        if (await repo.SaveChangesAsync())
        {
            return NoContent();
        }

        return BadRequest("Problem deleting the transaction status.");
    }

    private bool TransactionStatusExists(long id)
    {
        return repo.TransactionStatusExists(id);
    }
}