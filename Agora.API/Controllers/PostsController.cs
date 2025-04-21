using Agora.Core.Interfaces;
using Agora.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace Agora.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostsController(IPostRepository repo) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Post>>> GetAllPosts()
    {
        return Ok(await repo.GetAllPostsAsync());
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<Post>> GetPost([FromRoute] long id)
    {
        Post? post = await repo.GetPostByIdAsync(id);
        
        if (post == null) return NotFound();
        
        return Ok(post);
    }

    [HttpPost]
    public async Task<ActionResult<Post>> CreatePost([FromBody] Post post)
    {
        repo.AddPost(post);
        
        if (await repo.SaveChangesAsync())
        {
            return CreatedAtAction("GetPost", new { id = post.Id }, post);
        }
        
        return BadRequest("Problem creating the post.");
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult> UpdatePost([FromRoute] long id, [FromBody] Post post)
    {
        if (post.Id != id || !PostExists(id))
        {
            return BadRequest("Post doesn't exist or there is an incoherence between route and body ids.");
        }
        
        repo.UpdatePost(post);

        if (await repo.SaveChangesAsync())
        {
            return NoContent();
        }

        return BadRequest("Problem updating the post.");
    }

    [HttpDelete("{id:long}")]
    public async Task<ActionResult> DeletePost([FromRoute] long id)
    {
        Post? post = await repo.GetPostByIdAsync(id);

        if (post == null)
        {
            return NotFound();
        }

        repo.DeletePost(post);

        if (await repo.SaveChangesAsync())
        {
            return NoContent();
        }

        return BadRequest("Problem deleting the post.");
    }

    private bool PostExists(long id)
    {
        return repo.PostExists(id);
    }
}