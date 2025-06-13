using Agora.API.DTOs.PostCategory;
using Agora.API.InputValidation.Interfaces;
using Agora.API.QueryParams;
using Agora.Core.Constants;
using Agora.Core.Interfaces.Repositories;
using Agora.Core.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Agora.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostCategoriesController(
    IPostCategoryRepository repo, 
    IPostRepository postRepo,
    IMapper mapper,
    IInputValidator inputValidator) : ControllerBase
{
    private const string EntityName = "post category";
    
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
            ? NotFound(ErrorMessages.NotFound(EntityName)) 
            : Ok(mapper.Map<PostCategoryDetailsDto>(postCategory));
    }

    [Authorize(Roles = Roles.Admin)]
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
                return StatusCode(500, ErrorMessages.SavedButNotRetrieved(EntityName));
            }
            
            PostCategoryDetailsDto createdPostCategoryDetailsDto =
                mapper.Map<PostCategoryDetailsDto>(createdPostCategory);
            
            return CreatedAtAction(nameof(GetPostCategory), new { id = createdPostCategory.Id }, createdPostCategoryDetailsDto);
        }

        return BadRequest(ErrorMessages.UnknownErrorDuringAction(EntityName, "creation"));
    }

    [Authorize(Roles = Roles.Admin)]
    [HttpPut("{id:long}")]
    public async Task<ActionResult> UpdatePostCategory([FromRoute] long id, [FromBody] UpdatePostCategoryDto postCategoryDto)
    {
        // Cleaning
        postCategoryDto.Name = postCategoryDto.Name.Trim();
        
        // Retrieve the existing post category
        PostCategory? existingPostCategory = await repo.GetPostCategoryByIdAsync(id);
        if (existingPostCategory == null)
        {
            return NotFound(ErrorMessages.NotFound(EntityName));
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
            : BadRequest(ErrorMessages.UnknownErrorDuringAction(EntityName, "update"));
    }

    [Authorize(Roles = Roles.Admin)]
    [HttpDelete("{id:long}")]
    public async Task<ActionResult> DeletePost([FromRoute] long id)
    {
        PostCategory? postCategory = await repo.GetPostCategoryByIdAsync(id);

        if (postCategory == null)
        {
            return NotFound(ErrorMessages.NotFound(EntityName));
        }

        if (await postRepo.IsCategoryInUserAsync(id))
        {
            return BadRequest(ErrorMessages.PostCategory.InUse);
        }
        repo.DeletePostCategory(postCategory);

        return await repo.SaveChangesAsync()
            ? NoContent()
            : BadRequest(ErrorMessages.UnknownErrorDuringAction(EntityName, "deletion"));
    }
}