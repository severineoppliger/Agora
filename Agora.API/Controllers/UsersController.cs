using Agora.API.DTOs.User;
using Agora.Core.Interfaces;
using Agora.Core.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Agora.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(IUserRepository repo, IMapper mapper) : ControllerBase
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
        if (await repo.UsernameExistsAsync(userDto.Username))
            return BadRequest("Username is already taken.");

        if (await repo.EmailExistsAsync(userDto.Email))
            return BadRequest("Email is already in use.");
        
        User user = mapper.Map<User>(userDto);

        user.PasswordHash = userDto.Password; // TODO _authService.HashPassword(userDto.Password);
        user.CreatedAt = DateTime.UtcNow;
        user.Credit = 0;
        
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