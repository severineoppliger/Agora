using Agora.Core.Interfaces;
using Agora.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace Agora.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostCategoriesController(IPostCategoryRepository repo) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<PostCategory>>> GetAllPostCategories()
    {
        return Ok(await repo.GetAllPostCategoriesAsync());
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<PostCategory>> GetPostCategory([FromRoute] long id)
    {
        PostCategory? postCategory = await repo.GetPostCategoryByIdAsync(id);

        if (postCategory == null) return NotFound();
        return Ok(postCategory);
    }

    [HttpPost]
    public async Task<ActionResult<PostCategory>> CreatePostCategory([FromBody] PostCategory postCategory)
    {
        repo.AddPostCategory(postCategory);
        if (await repo.SaveChangesAsync())
        {
            return CreatedAtAction("GetPostCategory", new { id = postCategory.Id }, postCategory);
        }

        return BadRequest("Problem creating the post category.");
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult> UpdatePostCategory([FromRoute] long id, [FromBody] PostCategory postCategory)
    {
        if (postCategory.Id != id || !PostCategoryExists(id))
        {
            return BadRequest("Post category doesn't exist or there is an incoherence between route and body ids.");
        }

        repo.UpdatePostCategory(postCategory);

        if (await repo.SaveChangesAsync())
        {
            return NoContent();
        }

        return BadRequest("Problem updating the post category.");
    }

    [HttpDelete("{id:long}")]
    public async Task<ActionResult> DeletePost([FromRoute] long id)
    {
        PostCategory? postCategory = await repo.GetPostCategoryByIdAsync(id);

        if (postCategory == null)
        {
            return NotFound();
        }

        repo.DeletePostCategory(postCategory);

        if (await repo.SaveChangesAsync())
        {
            return NoContent();
        }

        return BadRequest("Problem deleting the post category");
    }

    private bool PostCategoryExists(long id)
    {
        return repo.PostCategoryExists(id);
    }
}