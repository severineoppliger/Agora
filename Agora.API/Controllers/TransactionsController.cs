using System.Security.Authentication;
using Agora.API.ApiQueryParameters;
using Agora.API.DTOs.Transaction;
using Agora.API.Extensions;
using Agora.API.Validation;
using Agora.API.Validation.Interfaces;
using Agora.Core.Commands;
using Agora.Core.Interfaces;
using Agora.Core.Interfaces.DomainServices;
using Agora.Core.Models;
using Agora.Core.Models.Entities;
using Agora.Core.Shared;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Agora.API.Controllers;

/// <summary>
/// Manages transactions between users, including creation, updates, and status changes.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class TransactionsController(
    IMapper mapper,
    IInputValidator inputValidator,
    ITransactionService transactionService,
    IUserContextService userContextService)
    : ControllerBase
{
    private const string EntityName = "transaction";

    /// <summary>
    /// Retrieves all transactions visible to the current user, optionally filtered by query parameters.
    /// </summary>
    /// <param name="queryParameters">Optional filters to apply to the transaction list.</param>
    /// <returns>Returns <c>200 OK</c> with a list of transaction summaries visible to the user.</returns>
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<TransactionSummaryDto>>> GetAllTransactions(
        [FromQuery] TransactionQueryParameters queryParameters)
    {
        // Extract current user's context from claims
        UserContext userContext;
        try
        {
            userContext = userContextService.GetCurrentUserContext();
        }
        catch (AuthenticationException ex)
        {
            return Unauthorized(ex.Message);
        }
        
        // Delegate business logic
        Result<IReadOnlyList<Transaction>> result = await transactionService.GetAllVisibleTransactionsAsync(queryParameters, userContext);
        
        if (result.IsFailure)
        {
            return this.MapErrorResult(result);
        }
        
        IReadOnlyList<Transaction> transactions = result.Value!;
        return Ok(mapper.Map<IReadOnlyList<TransactionSummaryDto>>(transactions));
    }

    /// <summary>
    /// Retrieves detailed information of a specific transaction by its identifier,
    /// if the current user is authorized to view it.
    /// </summary>
    /// <param name="id">The identifier of the transaction to retrieve.</param>
    /// <returns>
    /// Returns <c>200 OK</c> with the transaction details if found and authorized.
    /// Returns <c>401 Unauthorized</c> if the user is not allowed to view the transaction.
    /// Returns <c>404 Not Found</c> if the transaction does not exist.
    /// </returns>
    [Authorize]
    [HttpGet("{id:long}")]
    public async Task<ActionResult<TransactionDetailsDto>> GetTransaction([FromRoute] long id)
    {
        // Extract current user's context from claims
        UserContext userContext;
        try
        {
            userContext = userContextService.GetCurrentUserContext();
        }
        catch (AuthenticationException ex)
        {
            return Unauthorized(ex.Message);
        }

        // Delegate business logic
        Result<Transaction> result = await transactionService.GetVisibleTransactionByIdAsync(id, userContext);
        
        if (result.IsFailure)
        {
            return this.MapErrorResult(result);
        }

        // Return transaction if no error
        Transaction? transaction = result.Value;
        return transaction == null 
            ? NotFound(ErrorMessages.NotFound(EntityName, id.ToString()))
            : Ok(mapper.Map<TransactionDetailsDto>(transaction));
    }
    
    /// <summary>
    /// Creates a new transaction between two users.
    /// </summary>
    /// <param name="dto">The transaction data transfer object containing creation details.</param>
    /// <returns>
    /// Returns <c>201 Created</c> with the newly created transaction details.
    /// Returns <c>400 Bad Request</c> if input or business rules validation fails.
    /// Returns <c>401 Unauthorized</c> if the user is not authenticated.
    /// </returns>
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<TransactionDetailsDto>> CreateTransaction([FromBody] CreateTransactionDto dto)
    {
        // Validate input DTO
        InputValidationResult inputValidationResult = await inputValidator.ValidateCreateTransactionDtoAsync(dto);
        if (!inputValidationResult.IsValid)
        {
            return BadRequest(inputValidationResult.Errors);
        }
        
        // Extract current user's context from claims
        UserContext userContext;
        try
        {
            userContext = userContextService.GetCurrentUserContext();
        }
        catch (AuthenticationException ex)
        {
            return Unauthorized(ex.Message);
        }
        
        // Map the DTO to the full entity and delegate business logic (business rules + database changes)
        Transaction transaction = mapper.Map<Transaction>(dto);

        Result<Transaction> result = await transactionService.CreateTransactionAsync(transaction, userContext);

        if (result.IsFailure)
        {
            return this.MapErrorResult(result);
        }

        // Treat success case
        TransactionDetailsDto createdTransactionDetailsDto = mapper.Map<TransactionDetailsDto>(result.Value);
        return CreatedAtAction(nameof(GetTransaction), new { id = result.Value!.Id }, createdTransactionDetailsDto);
    }

    /// <summary>
    /// Updates details of an existing transaction partially.
    /// Only allowed if the current user has modification rights.
    /// </summary>
    /// <param name="id">The identifier of the transaction to update.</param>
    /// <param name="dto">Partial transaction data to update.</param>
    /// <returns>
    /// Returns <c>204 No Content</c> on successful update.
    /// Returns <c>400 Bad Request</c> if input validation fails.
    /// Returns <c>401 Unauthorized</c> if the user is not authenticated.
    /// Returns <c>403 Forbidden</c> if the user has not the modification rights for this transaction.
    /// Returns <c>404 Not Found</c> if the transaction or a related object does not exist.
    /// </returns>
    [Authorize]
    [HttpPatch("{id:long}")]
    public async Task<ActionResult> UpdateTransactionDetails([FromRoute] long id, [FromBody] UpdateTransactionDetailsDto dto)
    {
        // Validate input DTO
        InputValidationResult inputValidationResult = await inputValidator.ValidateUpdateTransactionDetailsDtoAsync(dto);
        if (!inputValidationResult.IsValid)
        {
            return BadRequest(inputValidationResult.Errors);
        }
        
        // Extract current user's context from claims
        UserContext userContext;
        try
        {
            userContext = userContextService.GetCurrentUserContext();
        }
        catch (AuthenticationException ex)
        {
            return Unauthorized(ex.Message);
        }
        
        // Delegate business logic (business rules + database changes)
        UpdateTransactionDetailsCommand newDetails = mapper.Map<UpdateTransactionDetailsCommand>(dto);
        Result result = await transactionService.UpdateTransactionDetailsAsync(id, newDetails, userContext);

        return result.IsFailure 
            ? this.MapErrorResult(result)
            : NoContent();
    }
    
    /// <summary>
    /// Changes the status of an existing transaction (e.g., from Pending to Accepted).
    /// This operation is allowed only for the buyer, seller, or administrators.
    /// </summary>
    /// <param name="id">The identifier of the transaction whose status is to be changed.</param>
    /// <param name="dto">The data transfer object containing the new status.</param>
    /// <returns>
    /// Returns <c>204 No Content</c> on successful status change.
    /// Returns <c>400 Bad Request</c> if input validation fails or status change is invalid.
    /// Returns <c>401 Unauthorized</c> if the user is not allowed to change the status.
    /// Returns <c>404 Not Found</c> if the transaction or a related object does not exist.
    /// </returns>
    [Authorize]
    [HttpPut("{id:long}/status")]
    public async Task<IActionResult> ChangeTransactionStatus(long id, [FromBody] ChangeTransactionStatusDto dto)
    {
        // Validate input DTO
        InputValidationResult inputValidationResult = inputValidator.ValidateChangeTransactionStatusDto(dto);
        if (!inputValidationResult.IsValid)
        {
            return BadRequest(inputValidationResult.Errors);
        }

        // Extract current user's context from claims
        UserContext userContext;
        try
        {
            userContext = userContextService.GetCurrentUserContext();
        }
        catch (AuthenticationException ex)
        {
            return Unauthorized(ex.Message);
        }
        
        // Delegate business logic (business rules + database changes)
        Result result = await transactionService.ChangeTransactionStatusAsync(id, userContext, dto.Status);

        return result.IsFailure 
            ? this.MapErrorResult(result)
            : NoContent();
    }
}