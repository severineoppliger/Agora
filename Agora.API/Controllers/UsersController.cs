using Agora.API.DTOs.User;
using Agora.API.InputValidation.Interfaces;
using Agora.API.Settings;
using Agora.Core.Constants;
using Agora.Core.Extensions;
using Agora.Core.Interfaces;
using Agora.Core.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Agora.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(
    SignInManager<AppUser> signInManager,
    IUserRepository repo,
    IOptions<UserSettings> userSettings,
    IMapper mapper,
    IInputValidator inputValidator) : ControllerBase

{
    private const string UserNotFoundMessage = "User not found.";
    private const string InvalidCredentialsMessage = "Invalid email or password.";
    private const string UserSavedButNotRetrievedMessage = "User was saved but could not be retrieved.";

    [Authorize(Roles = Roles.Admin)]
    [HttpGet("admin")]
    public async Task<ActionResult<IReadOnlyList<UserSummaryDto>>> GetAllUsers()
    {
        IReadOnlyList<AppUser> users = await repo.GetAllUsersAsync();
        return Ok(mapper.Map<IReadOnlyList<UserSummaryDto>>(users));
    }
    
    [Authorize(Roles = Roles.Admin)]
    [HttpGet("admin/{id}", Name = "GetUserById")]
    public async Task<ActionResult<UserDetailsDto>> GetUserByIdAsync([FromRoute] string id)
    {
        if (!id.IsGuid())
        {
            return BadRequest($"Invalid user ID format: {id}. Must be a valid GUID.");
        }

        AppUser? user = await repo.GetUserByIdAsync(id);

        return user == null
            ? NotFound(UserNotFoundMessage)
                : Ok(mapper.Map<UserDetailsDto>(user));
    }
    
    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<UserDetailsDto>> GetCurrentUserAsync()
    {
        string? userId = repo.GetUserId(User);
        if (userId is null)
        {
            return BadRequest(UserNotFoundMessage);
        }

        AppUser? user = await repo.GetUserByIdAsync(userId);
        
        return user == null
            ? NotFound(UserNotFoundMessage)
            : Ok(mapper.Map<UserDetailsDto>(user));
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDetailsDto>> Register([FromBody] RegisterDto registerDto)
    {
        // Cleaning
        registerDto.UserName = registerDto.UserName.Trim();
        registerDto.Email = registerDto.Email.Trim();
        registerDto.Password = registerDto.Password.Trim();

        // Input validation
        List<string> inputErrors = await inputValidator.ValidateInputRegisterDtoAsync(registerDto);
        if (inputErrors.Count != 0)
        {
            return BadRequest(new { Errors = inputErrors });
        }

        // Transform to the full entity (no business rule associated with user)
        AppUser user = mapper.Map<AppUser>(registerDto);

        user.CreatedAt = DateTime.UtcNow;
        user.Credit = userSettings.Value.InitialCredit;
        var result = await repo.AddUserAsync(user, registerDto.Password);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }

            return ValidationProblem();
        }

        AppUser? createdUser = await repo.GetUserByEmailAsync(registerDto.Email);

        if (createdUser == null)
        {
            return StatusCode(500, UserSavedButNotRetrievedMessage);
        }

        UserDetailsDto createdUserDetailsDto = mapper.Map<UserDetailsDto>(createdUser);

        return Ok(createdUserDetailsDto);
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] SignInDto signInDto)
    {
        AppUser? user = await repo.GetUserByEmailAsync(signInDto.Email);
        if (user == null)
        {
            return Unauthorized(InvalidCredentialsMessage);
        }

        var result =
            await signInManager.PasswordSignInAsync(user, signInDto.Password, isPersistent: false,
                lockoutOnFailure: false);

        if (!result.Succeeded)
        {
            return Unauthorized(InvalidCredentialsMessage);
        }
        return NoContent();
    }
    
    [Authorize]
    [HttpPost("logout")]
    public async Task<ActionResult> Logout()
    {
        await signInManager.SignOutAsync(); // Remove the cookie as well
        return NoContent();
    }
}