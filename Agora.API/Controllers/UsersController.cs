using System.Security.Authentication;
using Agora.API.ApiQueryParameters;
using Agora.API.DTOs.User;
using Agora.API.Extensions;
using Agora.API.Filters;
using Agora.API.Validation;
using Agora.API.Validation.Interfaces;
using Agora.Core.Commands;
using Agora.Core.Constants;
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
/// Handles operations related to user management such as registration, login, logout,
/// and retrieval of user information.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class UsersController(
    IAuthService authService,
    IMapper mapper,
    IUserService userService,
    IInputValidator inputValidator,
    IUserContextService userContextService) : ControllerBase

{

    /// <summary>
    /// Retrieves a list of all users in the system, optionally filtered and sorted using query parameters.
    /// </summary>
    /// <param name="queryParameters">Optional filtering and sorting parameters.</param>
    /// <returns>Returns <c>200 OK</c> with a list of summarized user information.
    /// Returns <c>403 Forbidden</c> if the user does not have admin privileges.</returns>
    [Authorize(Roles = Roles.Admin)]
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<UserSummaryDto>>> GetAllUsers([FromQuery] UserQueryParameters queryParameters)
    {
        // Delegate business logic
        Result<IReadOnlyList<User>> result = await userService.GetAllUsersAsync(queryParameters);
        IReadOnlyList<User> users = result.Value!;
        
        return Ok(mapper.Map<IReadOnlyList<UserSummaryDto>>(users));
    }
    
    /// <summary>
    /// Retrieves detailed information about a specific user by their ID.
    /// </summary>
    /// <param name="id">The unique identifier (GUID) of the user.</param>
    /// <returns>
    /// Returns <c>200 OK</c> with the user's details.
    /// Returns <c>400 BadRequest</c> if the ID format is invalid.
    /// Returns <c>401 Unauthorized</c> if the user is not authenticated.
    /// Returns <c>403 Forbidden</c> if the user does not have admin privileges.
    /// Returns <c>404 Not Found</c> if the user cannot be retrieved.
    /// </returns>
    [Authorize(Roles = Roles.Admin)]
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDetailsDto>> GetUser([FromRoute] string id)
    {
        // Validate input id
        InputValidationResult inputValidationResult = inputValidator.ValidateUserId(id);
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
        
        // Delegate business logic
        Result<User> result = await userService.GetUserByIdAsync(id, userContext);

        return result.IsFailure 
            ? this.MapErrorResult(result)
            : Ok(mapper.Map<UserDetailsDto>(result.Value));
    }
    
    /// <summary>
    /// Retrieves Email address of a specific user, knowing its username.
    /// </summary>
    /// <param name="userName">The userName of the user to retrieve.</param>
    /// <returns>
    /// Returns <c>200 OK</c> with the user's email.
    /// Returns <c>401 Unauthorized</c> if the user is not authenticated.
    /// Returns <c>404 Not Found</c> if the user cannot be retrieved.
    /// </returns>
    [Authorize]
    [HttpGet("{userName}/email")]
    public async Task<ActionResult<UserEmailDto>> GetUserEmail([FromRoute] string userName)
    {
        // Delegate business logic
        Result<User> result = await userService.GetUserEmailByUsernameAsync(userName);

        return result.IsFailure 
            ? this.MapErrorResult(result)
            : Ok(mapper.Map<UserEmailDto>(result.Value));
    }
    
    /// <summary>
    /// Retrieves the currently authenticated user's details.
    /// </summary>
    /// <returns>
    /// Returns <c>200 OK</c> with the authenticated user's details.
    /// Returns <c>401 Unauthorized</c> if the user is not authenticated.
    /// Returns <c>404 Not Found</c> if the user cannot be retrieved.
    /// </returns>
    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<UserDetailsDto>> GetCurrentUserAsync()
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
        Result<User> result = await userService.GetUserByIdAsync(userContext.UserId, userContext);

        return result.IsFailure 
            ? this.MapErrorResult(result)
            : Ok(mapper.Map<UserDetailsDto>(result.Value));
    }

    /// <summary>
    /// Registers a new <c>User</c> in the system.
    /// </summary>
    /// <param name="userDto">The registration data including username, email, and password.</param>
    /// <returns>
    /// Returns <c>200 OK</c> with the new user's details, if the user was successfully registered.
    /// Returns <c>400 BadRequest</c> if input validation fails or the user already exists.
    /// Returns <c>500 Internal Server Error</c> if the user was saved but could not be retrieved afterwards.
    /// </returns>
    [HttpPost("register")]
    [DisallowAuthenticated]
    public async Task<ActionResult<UserDetailsDto>> Register([FromBody] RegisterUserDto userDto)
    {
        // Validate input DTO
        InputValidationResult inputValidationResult = await inputValidator.ValidateRegisterDtoAsync(userDto);
        if (!inputValidationResult.IsValid)
        {
            return BadRequest(inputValidationResult.Errors);
        }

        // Delegate business logic (business rules + database changes)
        RegisterUserCommand registerUserCommand = mapper.Map<RegisterUserCommand>(userDto);
        Result<User> result = await authService.RegisterAsync(registerUserCommand);

        return result.IsFailure
            ? this.MapErrorResult(result)
            : Ok(mapper.Map<UserDetailsDto>(result.Value));
    }
    
    /// <summary>
    /// Authenticates a user and starts a session.
    /// </summary>
    /// <param name="userDto">The login credentials: email and password.</param>
    /// <returns>
    /// Returns <c>204 No Content</c> on successful login.
    /// Returns <c>401 Unauthorized</c> if the credentials are invalid.
    /// </returns>
    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] SignInUserDto userDto)
    {
        SignInUserCommand command = mapper.Map<SignInUserCommand>(userDto);
        Result result = await authService.LoginAsync(command);
        return result.IsFailure 
            ? this.MapErrorResult(result)
            : NoContent();
    }
    
    /// <summary>
    /// Logs out the currently authenticated user.
    /// </summary>
    /// <returns>
    /// Returns <c>204 No Content</c> on successful logout.
    /// Returns <c>401 Unauthorized</c> if the user was not authenticated.
    /// </returns>
    [Authorize]
    [HttpPost("logout")]
    public async Task<ActionResult> Logout()
    {
        await authService.LogoutAsync();
        return NoContent();
    }
}