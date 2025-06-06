using Agora.API.DTOs.User;
using Agora.API.InputValidation.Interfaces;
using Agora.API.Settings;
using Agora.Core.Constants;
using Agora.Core.Extensions;
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
    UserManager<AppUser> userManager,
    IOptions<UserSettings> userSettings,
    IMapper mapper,
    IInputValidator inputValidator) : ControllerBase

{
    private const string UserNotFoundMessage = "User not found.";

    [HttpGet("admin")]
    public async Task<ActionResult<IReadOnlyList<UserSummaryDto>>> GetAllUsers()
    {
        IReadOnlyList<AppUser> users = await userManager.Users.ToListAsync();
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
        
        AppUser? user = await userManager.Users.FirstOrDefaultAsync(u => u.Id == id);

        return user == null
            ? NotFound(UserNotFoundMessage)
                : Ok(mapper.Map<UserDetailsDto>(user));
    }
    
    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<UserDetailsDto>> GetCurrentUserAsync()
    {
        AppUser? user = await userManager.GetUserAsync(User);
        
        return Ok(mapper.Map<UserDetailsDto>(user));
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
        var result = await signInManager.UserManager.CreateAsync(user, registerDto.Password);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }

            return ValidationProblem();
        }

        AppUser? createdUser = await userManager.FindByEmailAsync(registerDto.Email);

        if (createdUser == null)
        {
            return StatusCode(500, "User was saved but could not be retrieved.");
        }

        UserDetailsDto createdUserDetailsDto = mapper.Map<UserDetailsDto>(createdUser);

        return Ok(createdUserDetailsDto);
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] SignInDto signInDto)
    {
        AppUser? user = await userManager.FindByEmailAsync(signInDto.Email);
        if (user == null)
        {
            return Unauthorized("Invalid email or password.");
        }

        var result =
            await signInManager.PasswordSignInAsync(user, signInDto.Password, isPersistent: false,
                lockoutOnFailure: false);

        if (!result.Succeeded)
        {
            return Unauthorized("Invalid email or password.");
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