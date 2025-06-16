using Agora.API.DTOs.TransactionStatus;
using Agora.API.Extensions;
using Agora.API.QueryParams;
using Agora.Core.Common;
using Agora.Core.Constants;
using Agora.Core.Interfaces.BusinessServices;
using Agora.Core.Models;
using Agora.Core.Models.Requests;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Agora.API.Controllers;

/// <summary>
/// Handles operations related to transaction status management,
/// such as updating status definitions or retrieving status information.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class TransactionStatusController(
    IMapper mapper,
    ITransactionStatusService transactionStatusService) : ControllerBase
{
    /// <summary>
    /// Retrieves all transaction status, optionally filtered and sorted by query parameters.
    /// </summary>
    /// <param name="queryParameters">Optional filters to apply to the transaction status list.</param>
    /// <returns>Returns <c>200 OK</c> with a list of transaction status.</returns>
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<TransactionStatusSummaryDto>>> GetAllTransactionStatus([FromQuery] TransactionStatusQueryParameters queryParameters)
    {
        // Delegate business logic
        Result<IReadOnlyList<TransactionStatus>> result = await transactionStatusService.GetAllTransactionStatusAsync(queryParameters);
        IReadOnlyList<TransactionStatus> transactions = result.Value!;
        
        return Ok(mapper.Map<IReadOnlyList<TransactionStatusSummaryDto>>(transactions));
    }

    /// <summary>
    /// Retrieves detailed information of a specific transaction status by its identifier, like all related transactions.
    /// </summary>
    /// <param name="id">The identifier of the transaction status to retrieve.</param>
    /// <returns>
    /// Returns <c>200 OK</c> with the transaction status details if found and authorized.
    /// Returns <c>401 Unauthorized</c> if the user is not authenticated.
    /// Returns <c>403 Forbidden</c> if the user is not allowed to view the transaction status (not admin).
    /// Returns <c>404 Not Found</c> if the transaction status does not exist.
    /// </returns>
    [Authorize(Roles = Roles.Admin)]
    [HttpGet("{id:long}")]
    public async Task<ActionResult<TransactionStatusDetailsDto>> GetTransactionStatus([FromRoute] long id)
    {
        // Delegate business logic
        Result<TransactionStatus> result = await transactionStatusService.GetTransactionStatusByIdAsync(id);
        
        return result.IsFailure
            ? this.MapErrorResult(result)
            : Ok(mapper.Map<TransactionStatusDetailsDto>(result.Value));
    }
    
    
    // TODO Change name
    /// <summary>
    /// Updates details of an existing transaction status partially (name and/or description).
    /// Only allowed if the current user is an admin.
    /// </summary>
    /// <param name="id">The identifier of the transaction status to update.</param>
    /// <param name="dto">Partial transaction status data to update.</param>
    /// <returns>
    /// Returns <c>204 No Content</c> on successful update.
    /// Returns <c>400 Bad Request</c> if input validation fails.
    /// Returns <c>401 Unauthorized</c> if the user is not authenticated.
    /// Returns <c>403 Forbidden</c> if the user has not the modification rights.
    /// Returns <c>404 Not Found</c> if the transaction status or a related object does not exist.
    /// </returns>
    [Authorize(Roles = Roles.Admin)]
    [HttpPut("{id:long}")]
    public async Task<ActionResult> UpdateTransactionStatus([FromRoute] long id, [FromBody] UpdateTransactionStatusDetailsDto dto)
    {
        // Delegate business logic (business rules + database changes)
        TransactionStatusDetailsUpdate newDetails = mapper.Map<TransactionStatusDetailsUpdate>(dto);
        Result result = await transactionStatusService.UpdateTransactionStatusDetailsAsync(id, newDetails);

        return result.IsFailure 
            ? this.MapErrorResult(result)
            : NoContent();

    }
}