using Agora.Core.Interfaces;
using Agora.Core.Models;
using Agora.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Agora.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostsController(IPostRepository postRepo) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Post>>> GetPosts()
    {
        return Ok(await postRepo.GetPostsAsync());
    }

    [HttpGet("id:long")]
    public async Task<ActionResult<Post>> GetPost(long id)
    {
        Post? post = await postRepo.GetPostByIdAsync(id);

        if (post == null) return NotFound();
        return Ok(post);
    }

    [HttpPost]
    public async Task<ActionResult<Post>> CreatePost(Post post)
    {
        postRepo.AddPost(post);

        if (await postRepo.SaveChangesAsync())
        {
            return CreatedAtAction("GetPost", new { id = post.Id }, post);
        }
        return BadRequest("Problem creating post");
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdatePost([FromRoute] long id, [FromBody] Post post)
    {
        if (post.Id != id || !PostExists(id))
        {
            return BadRequest("Post doesn't exist or incoherence between route and body ids.");
        }
        
        postRepo.UpdatePost(post);

        if (await postRepo.SaveChangesAsync())
        {
            return NoContent();
        }

        return BadRequest("Problem updating the post.");
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeletePost([FromRoute] long id)
    {
        Post? post = await postRepo.GetPostByIdAsync(id);

        if (post == null)
        {
            return NotFound();
        }

        postRepo.DeletePost(post);

        if (await postRepo.SaveChangesAsync())
        {
            return NoContent();
        }

        return BadRequest("Problem deleting the post.");
    }

    private bool PostExists(long id)
    {
        return postRepo.PostExists(id);
    }
}