using Agora.API.DTOs.TransactionStatus;
using Agora.API.InputValidation.Interfaces;
using Agora.Core.Interfaces;
using Agora.Core.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Agora.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionStatusController(
    ITransactionStatusRepository repo, 
    IMapper mapper,
    IInputValidator inputValidator) : ControllerBase
{
    private const string TransactionStatusNotFoundMessage = "Transaction status not found.";

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
        
        return transactionStatus == null 
            ? NotFound(TransactionStatusNotFoundMessage)
            : Ok(mapper.Map<TransactionStatusDetailsDto>(transactionStatus));
    }

    [HttpPost]
    public async Task<ActionResult<TransactionStatusDetailsDto>> CreateTransactionStatus([FromBody] CreateTransactionStatusDto transactionStatusDto)
    {
        transactionStatusDto.Name = transactionStatusDto.Name.Trim();
        
        // Input validation
        List<string> inputErrors = await inputValidator.ValidateInputTransactionStatusDtoAsync(transactionStatusDto);
        if (inputErrors.Count != 0)
            return BadRequest(new { Errors = inputErrors });
        
        // Transform to the full entity (no business rule associated with transaction status)
        TransactionStatus transactionStatus = mapper.Map<TransactionStatus>(transactionStatusDto);
        
        // Add to database
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
        transactionStatusDto.Name = transactionStatusDto.Name.Trim();

        // Retrieve the existing transaction status
        TransactionStatus? existingTransactionStatus = await repo.GetTransactionStatusByIdAsync(id);
        if (existingTransactionStatus == null) return NotFound(TransactionStatusNotFoundMessage);
        
        // Input validation
        List<string> inputErrors = await inputValidator.ValidateInputTransactionStatusDtoAsync(transactionStatusDto, existingTransactionStatus.Name);
        if (inputErrors.Count != 0)
            return BadRequest(new { Errors = inputErrors });
        
        // Apply the updated fields exposed in the DTO to the existing transaction status
        mapper.Map(transactionStatusDto, existingTransactionStatus);
        
        return await repo.SaveChangesAsync()
            ? NoContent()
            : BadRequest("Problem updating the transaction status.");
    }

    [HttpDelete("{id:long}")]
    public async Task<ActionResult> DeleteTransactionStatus([FromRoute] long id)
    {
        TransactionStatus? transactionStatus = await repo.GetTransactionStatusByIdAsync(id);

        if (transactionStatus == null)
        {
            return NotFound(TransactionStatusNotFoundMessage);
        }

        repo.DeleteTransactionStatus(transactionStatus);

        return await repo.SaveChangesAsync()
            ? NoContent()
            : BadRequest("Problem deleting the transaction status.");
    }
}