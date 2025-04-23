using Agora.Core.Interfaces;
using Agora.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace Agora.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(IUserRepository repo) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<User>>> GetAllUsers()
    {
        return Ok(await repo.GetAllUsersAsync());
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<User>> GetUser([FromRoute] long id)
    {
        User? user = await repo.GetUserByIdAsync(id);

        if (user == null) return NotFound();

        return Ok(user);
    }
    
    [HttpPost]
    public async Task<ActionResult<User>> CreateUser([FromBody] User user)
    {
        repo.AddUser(user);
        
        if (await repo.SaveChangesAsync())
        {
            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }
        
        return BadRequest("Problem creating the user.");
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult> UpdateUser([FromRoute] long id, [FromBody] User user)
    {
        if (user.Id != id || !UserExists(id))
        {
            return BadRequest("User doesn't exist or there is an incoherence between route and body ids.");
        }
        
        repo.UpdateUser(user);

        if (await repo.SaveChangesAsync())
        {
            return NoContent();
        }

        return BadRequest("Problem updating the user.");
    }

    [HttpDelete("{id:long}")]
    public async Task<ActionResult> DeleteUser([FromRoute] long id)
    {
        User? user = await repo.GetUserByIdAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        repo.DeleteUser(user);

        if (await repo.SaveChangesAsync())
        {
            return NoContent();
        }

        return BadRequest("Problem deleting the user.");
    }

    private bool UserExists(long id)
    {
        return repo.UserExists(id);
    }
}