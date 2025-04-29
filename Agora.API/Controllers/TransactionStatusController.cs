using Agora.API.DTOs.TransactionStatus;
using Agora.Core.Interfaces;
using Agora.Core.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Agora.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionStatusController(ITransactionStatusRepository repo, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<TransactionStatusSummaryDto>>> GetAllTransactionStatus()
    {
        IReadOnlyList <TransactionStatus> transactionStatusList = await repo.GetAllTransactionStatusAsync();
        return Ok(mapper.Map<IReadOnlyList<TransactionStatusSummaryDto>>(transactionStatusList));
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<TransactionStatusDetailsDto>> GetTransactionStatus([FromRoute] long id)
    {
        TransactionStatus? transactionStatus = await repo.GetTransactionStatusByIdAsync(id);
        
        if (transactionStatus == null) return NotFound();
        
        return Ok(mapper.Map<TransactionStatusDetailsDto>(transactionStatus));
    }

    [HttpPost]
    public async Task<ActionResult<TransactionStatusDetailsDto>> CreateTransactionStatus([FromBody] CreateTransactionStatusDto transactionStatusDto)
    {
        TransactionStatus transactionStatus = mapper.Map<TransactionStatus>(transactionStatusDto);
        repo.AddTransactionStatus(transactionStatus);

        if (await repo.SaveChangesAsync())
        {
            TransactionStatus? createdTransactionStatus = await repo.GetTransactionStatusByIdAsync(transactionStatus.Id);

            if (createdTransactionStatus == null)
            {
                return StatusCode(500, "Transaction status was saved but could not be retrieved.");
            }

            TransactionStatusDetailsDto createdTransactionStatusDetailsDto = mapper.Map<TransactionStatusDetailsDto>(createdTransactionStatus);
            
            return CreatedAtAction(nameof(GetTransactionStatus), new { id = createdTransactionStatus.Id }, createdTransactionStatusDetailsDto);
        }

        return BadRequest("Problem creating the transaction status.");
    } 
    
    [HttpPut("{id:long}")]
    public async Task<ActionResult> UpdateTransactionStatus([FromRoute] long id, [FromBody] UpdateTransactionStatusDto transactionStatusDto)
    {
        TransactionStatus? existingTransactionStatus = await repo.GetTransactionStatusByIdAsync(id);

        if (existingTransactionStatus == null) return NotFound();
        
        // Apply the updated fields exposed in the DTO to the existing transaction status
        mapper.Map(transactionStatusDto, existingTransactionStatus);
        
        if (await repo.SaveChangesAsync()) return NoContent();
        
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
}