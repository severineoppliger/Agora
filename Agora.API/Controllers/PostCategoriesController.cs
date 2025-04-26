using Agora.API.DTOs.PostCategory;
using Agora.Core.Interfaces;
using Agora.Core.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Agora.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostCategoriesController(IPostCategoryRepository repo, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<PostCategorySummaryDto>>> GetAllPostCategories()
    {
        IReadOnlyList<PostCategory> postsCategories = await repo.GetAllPostCategoriesAsync();
        return Ok(mapper.Map<IReadOnlyList<PostCategorySummaryDto>>(postsCategories));
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<PostCategoryDetailsDto>> GetPostCategory([FromRoute] long id)
    {
        PostCategory? postCategory = await repo.GetPostCategoryByIdAsync(id);

        if (postCategory == null) return NotFound();
        return Ok(mapper.Map<PostCategoryDetailsDto>(postCategory));
    }

    [HttpPost]
    public async Task<ActionResult<PostCategoryDetailsDto>> CreatePostCategory([FromBody] CreatePostCategoryDto postCategoryDto)
    {
        PostCategory postCategory = mapper.Map<PostCategory>(postCategoryDto);
        
        repo.AddPostCategory(postCategory);
        
        if (await repo.SaveChangesAsync())
        {
            PostCategory? createdPostCategory = await repo.GetPostCategoryByIdAsync(postCategory.Id);
            
            if (createdPostCategory == null)
            {
                return StatusCode(500, "Post category was saved but could not be retrieved.");
            }
            
            PostCategoryDetailsDto createdPostCategoryDetailsDto =
                mapper.Map<PostCategoryDetailsDto>(createdPostCategory);
            
            return CreatedAtAction("GetPostCategory", new { id = createdPostCategory.Id }, createdPostCategoryDetailsDto);
        }

        return BadRequest("Problem creating the post category.");
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult> UpdatePostCategory([FromRoute] long id, [FromBody] UpdatePostCategoryDto postCategoryDto)
    {
        PostCategory? existingPostCategory = await repo.GetPostCategoryByIdAsync(id);

        if (existingPostCategory == null) return NotFound();
        
        // Apply the updated fields exposed in the DTO to the existing post category
        mapper.Map(postCategoryDto, existingPostCategory);

        if (await repo.SaveChangesAsync()) return NoContent();

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
}