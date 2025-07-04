﻿using Agora.API.DTOs.Post;
using Agora.API.DTOs.Transaction;
using Agora.API.DTOs.TransactionStatus;
using Agora.API.DTOs.User;

namespace Agora.API.Validation.Interfaces;

/// <summary>
/// Interface defining methods to validate user input data before processing.
/// Ensures input DTOs respect expected formats, required constraints, 
/// and references to existing entities.
/// </summary>
public interface IInputValidator
{
    /// <summary>
    /// Validates the input data for a new <c>User</c> registration.
    /// Checks for username and email uniqueness.
    /// </summary>
    /// <param name="userDto">The DTO containing registration data.</param>
    /// <returns>An <see cref="InputValidationResult"/> containing any validation errors.</returns>
    public Task<InputValidationResult> ValidateRegisterDtoAsync(RegisterUserDto userDto);

    /// <summary>
    /// Validates that the user id respects the required format (GUID).
    /// </summary>
    /// <param name="id">The user id to check</param>
    /// <returns>An <see cref="InputValidationResult"/> containing any validation errors.</returns>
    public InputValidationResult ValidateUserId(string id);

    /// <summary>
    /// Validates the input data for creating a new <c>Post</c>.
    /// Ensures the post type is valid and the specified post category exists.
    /// </summary>
    /// <param name="dto">The DTO containing post creation data.</param>
    /// <returns>An <see cref="InputValidationResult"/> containing any validation errors.</returns>
    public Task<InputValidationResult> ValidateCreatePostDtoAsync(CreatePostDto dto);
    
    /// <summary>
    /// Validates the input data for updating an existing <c>Post</c>.
    /// Checks for valid post type and verifies the existence of the specified post category if provided.
    /// </summary>
    /// <param name="dto">The DTO containing updated post details.</param>
    /// <returns>An <see cref="InputValidationResult"/> containing any validation errors.</returns>
    public Task<InputValidationResult> ValidateUpdatePostDtoAsync(UpdatePostDetailsDto dto);

    /// <summary>
    /// Validates the input data for updating an existing <c>TransactionStatus</c>.
    /// </summary>
    /// <param name="dto">The DTO containing updated transaction status details.</param>
    /// <returns>An <see cref="InputValidationResult"/> containing any validation errors.</returns>
    public InputValidationResult ValidateUpdateTransactionStatusDtoAsync(UpdateTransactionStatusDetailsDto dto);
    
    /// <summary>
    /// Validates the input data for creating a new <c>Transaction</c>.
    /// Ensures that the referenced post (if any) exists, and that both buyer and seller users are valid.
    /// </summary>
    /// <param name="dto">The DTO containing transaction creation data.</param>
    /// <returns>An <see cref="InputValidationResult"/> containing any validation errors.</returns>
    public Task<InputValidationResult> ValidateCreateTransactionDtoAsync(CreateTransactionDto dto);
    
    /// <summary>
    /// Validates the input data for updating an existing <c>Transaction</c>.
    /// Ensures the referenced post (if any) exists.
    /// </summary>
    /// <param name="dto">The DTO containing updated transaction data.</param>
    /// <returns>An <see cref="InputValidationResult"/> containing any validation errors.</returns>
    public Task<InputValidationResult> ValidateUpdateTransactionDetailsDtoAsync(UpdateTransactionDetailsDto dto);
    
    /// <summary>
    /// Validates the input data for changing the status of a <c>Transaction</c>.
    /// Ensures that the provided new status exists in <c>TransactionStatusEnum</c>.
    /// </summary>
    /// <param name="dto">The DTO containing the requested status change.</param>
    /// <returns>An <see cref="InputValidationResult"/> containing any validation errors.</returns>
    public InputValidationResult ValidateChangeTransactionStatusDto(ChangeTransactionStatusDto dto);
}