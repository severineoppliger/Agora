using Agora.API.DTOs.PostCategory;
using Agora.API.InputValidation.Interfaces;
using Agora.API.QueryParams;
using Agora.Core.Interfaces;
using Agora.Core.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Agora.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostCategoriesController(
    IPostCategoryRepository repo, 
    IMapper mapper,
    IInputValidator inputValidator) : ControllerBase
{
    private const string PostCategoryNotFoundMessage = "Post category not found.";
    
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<PostCategorySummaryDto>>> GetAllPostCategories([FromQuery] PostCategoryQueryParameters queryParameters)
    {
        IReadOnlyList<PostCategory> postsCategories = await repo.GetAllPostCategoriesAsync(queryParameters);
        return Ok(mapper.Map<IReadOnlyList<PostCategorySummaryDto>>(postsCategories));
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<PostCategoryDetailsDto>> GetPostCategory([FromRoute] long id)
    {
        PostCategory? postCategory = await repo.GetPostCategoryByIdAsync(id);

        return postCategory == null 
            ? NotFound(PostCategoryNotFoundMessage) 
            : Ok(mapper.Map<PostCategoryDetailsDto>(postCategory));
    }

    [HttpPost]
    public async Task<ActionResult<PostCategoryDetailsDto>> CreatePostCategory([FromBody] CreatePostCategoryDto postCategoryDto)
    {
        // Cleaning
        postCategoryDto.Name = postCategoryDto.Name.Trim();
        
        // Input validation
        List<string> inputErrors = await inputValidator.ValidateInputPostCategoryDtoAsync(postCategoryDto);
        if (inputErrors.Count != 0)
        {
            return BadRequest(new { Errors = inputErrors });
        }

        // Transform to the full entity (no business rule associated with post category)
        PostCategory postCategory = mapper.Map<PostCategory>(postCategoryDto);
        
        // Add to database
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
            
            return CreatedAtAction(nameof(GetPostCategory), new { id = createdPostCategory.Id }, createdPostCategoryDetailsDto);
        }

        return BadRequest("Problem creating the post category.");
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult> UpdatePostCategory([FromRoute] long id, [FromBody] UpdatePostCategoryDto postCategoryDto)
    {
        // Cleaning
        postCategoryDto.Name = postCategoryDto.Name.Trim();
        
        // Retrieve the existing post category
        PostCategory? existingPostCategory = await repo.GetPostCategoryByIdAsync(id);
        if (existingPostCategory == null)
        {
            return NotFound(PostCategoryNotFoundMessage);
        }

        // Input validation
        List<string> inputErrors = await inputValidator.ValidateInputPostCategoryDtoAsync(postCategoryDto, existingPostCategory.Name);
        if (inputErrors.Count != 0)
        {
            return BadRequest(new { Errors = inputErrors });
        }

        // Apply the updated fields exposed in the DTO to the existing post category
        mapper.Map(postCategoryDto, existingPostCategory);

        return await repo.SaveChangesAsync()
            ? NoContent()
            : BadRequest("Problem updating the post category.");
    }

    [HttpDelete("{id:long}")]
    public async Task<ActionResult> DeletePost([FromRoute] long id)
    {
        PostCategory? postCategory = await repo.GetPostCategoryByIdAsync(id);

        if (postCategory == null)
        {
            return NotFound(PostCategoryNotFoundMessage);
        }

        repo.DeletePostCategory(postCategory);

        return await repo.SaveChangesAsync()
            ? NoContent()
            : BadRequest("Problem deleting the post category");
    }
}