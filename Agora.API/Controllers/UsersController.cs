using Agora.API.DTOs.User;
using Agora.API.InputValidation.Interfaces;
using Agora.Core.Interfaces;
using Agora.Core.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Agora.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(
    IUserRepository repo,
    IMapper mapper,
    IInputValidator inputValidator) : ControllerBase
{
    private const string UserNotFoundMessage = "User not found.";

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<UserSummaryDto>>> GetAllUsers()
    {
        IReadOnlyList<User> users = await repo.GetAllUsersAsync();
        return Ok(mapper.Map<IReadOnlyList<UserSummaryDto>>(users));
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<UserDetailsDto>> GetUser([FromRoute] long id)
    {
        User? user = await repo.GetUserByIdAsync(id);

        return user == null
            ? NotFound(UserNotFoundMessage)
            : Ok(mapper.Map<UserDetailsDto>(user));
    }
    
    [HttpPost]
    public async Task<ActionResult<UserDetailsDto>> CreateUser([FromBody] CreateUserDto userDto)
    {
        // Cleaning
        userDto.Username = userDto.Username.Trim();
        userDto.Email = userDto.Email.Trim();
        userDto.Password = userDto.Password.Trim();
        
        // Input validation
        List<string> inputErrors = await inputValidator.ValidateInputUserDtoAsync(userDto);
        if (inputErrors.Count != 0)
            return BadRequest(new { Errors = inputErrors });
        
        // Transform to the full entity (no business rule associated with user)
        User user = mapper.Map<User>(userDto);

        user.PasswordHash = userDto.Password; // TODO _authService.HashPassword(userDto.Password);
        user.CreatedAt = DateTime.UtcNow;
        user.Credit = 0; // TODO initialize the credit to some configurable amount.
        
        repo.AddUser(user);
        
        if (await repo.SaveChangesAsync())
        {
            User? createdUser = await repo.GetUserByIdAsync(user.Id);
            
            if (createdUser == null)
            {
                return StatusCode(500, "User was saved but could not be retrieved.");
            }
            
            UserDetailsDto createdUserDto = mapper.Map<UserDetailsDto>(createdUser);
            
            return CreatedAtAction(nameof(GetUser), new { id = createdUser.Id }, createdUserDto);
        }
        
        return BadRequest("Problem creating the user.");
    }

    [HttpDelete("{id:long}")]
    public async Task<ActionResult> DeleteUser([FromRoute] long id)
    {
        User? user = await repo.GetUserByIdAsync(id);

        if (user == null)
        {
            return NotFound(UserNotFoundMessage);
        }

        repo.DeleteUser(user);

        return await repo.SaveChangesAsync()
            ? NoContent()
            : BadRequest("Problem deleting the user.");
    }
}