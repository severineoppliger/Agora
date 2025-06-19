using Agora.API.ApiQueryParameters;
using Agora.API.DTOs.PostCategory;
using Agora.API.Extensions;
using Agora.Core.Constants;
using Agora.Core.Interfaces.DomainServices;
using Agora.Core.Models.Entities;
using Agora.Core.Shared;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Agora.API.Controllers;

/// <summary>
/// Manages operations for post categories, including listing and retrieving post category data.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class PostCategoriesController(
    IPostCategoryService postCategoryService,
    IMapper mapper) : ControllerBase
{
    private const string EntityName = "post category";
    
    /// <summary>
    /// Retrieves all post categories, optionally filtered and sorted by query parameters.
    /// </summary>
    /// <param name="queryParameters">Optional filters to apply to the post categories list.</param>
    /// <returns>Returns <c>200 OK</c> with a list of post categories.</returns>
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<PostCategorySummaryDto>>> GetAllPostCategories([FromQuery] PostCategoryQueryParameters queryParameters)
    {
        // Delegate business logic
        Result<IReadOnlyList<PostCategory>> result = await postCategoryService.GetAllPostCategoriesAsync(queryParameters);
        IReadOnlyList<PostCategory> postCategories = result.Value!;
        
        return Ok(mapper.Map<IReadOnlyList<PostCategorySummaryDto>>(postCategories));
    }

    /// <summary>
    /// Retrieves detailed information of a specific post category by its identifier, like all related posts.
    /// </summary>
    /// <param name="id">The identifier of the post category to retrieve.</param>
    /// <returns>
    /// Returns <c>200 OK</c> with the post category details if found and authorized.
    /// Returns <c>401 Unauthorized</c> if the user is not authenticated.
    /// Returns <c>403 Forbidden</c> if the user is not allowed to view the post category (not admin).
    /// Returns <c>404 Not Found</c> if the post category does not exist.
    /// </returns>
    [Authorize(Roles = Roles.Admin)]
    [HttpGet("{id:long}")]
    public async Task<ActionResult<PostCategoryDetailsDto>> GetPostCategory([FromRoute] long id)
    {
        // Delegate business logic
        Result<PostCategory> result = await postCategoryService.GetPostCategoryByIdAsync(id);
        
        if (result.IsFailure)
        {
            return this.MapErrorResult(result);
        }

        // Return post category if no error
        PostCategory? postCategory = result.Value;
        return postCategory == null 
            ? NotFound(ErrorMessages.NotFound(EntityName, id.ToString()))
            : Ok(mapper.Map<PostCategoryDetailsDto>(postCategory));
    }

    /// <summary>
    /// Creates a new post category.
    /// </summary>
    /// <param name="dto">The post category data transfer object containing creation details.</param>
    /// <returns>
    /// Returns <c>201 Created</c> with the newly created post category details.
    /// Returns <c>400 Bad Request</c> if input or business rules validation fails.
    /// Returns <c>401 Unauthorized</c> if the user is not authenticated.
    /// Returns <c>403 Forbidden</c> if the user is not allowed to manage the post category (not admin).
    /// </returns>
    [Authorize(Roles = Roles.Admin)]
    [HttpPost]
    public async Task<ActionResult<PostCategoryDetailsDto>> CreatePostCategory([FromBody] CreatePostCategoryDto dto)
    {
        // Map the DTO to the full entity and delegate business logic (business rules + database changes)
        PostCategory postCategory = mapper.Map<PostCategory>(dto);

        Result<PostCategory> result = await postCategoryService.CreatePostCategoryAsync(postCategory);
        if (result.IsFailure)
        {
            return this.MapErrorResult(result);
        }
        
        // Treat success case
        PostCategoryDetailsDto createdPostCategoryDetailsDto = mapper.Map<PostCategoryDetailsDto>(result.Value);
        return CreatedAtAction(nameof(GetPostCategory), new { id = result.Value!.Id },
            createdPostCategoryDetailsDto);
    }

    /// <summary>
    /// Updates name of an existing post category.
    /// </summary>
    /// <param name="id">The identifier of the post category to update.</param>
    /// <param name="dto">Dto containing the new name to apply to the post category.</param>
    /// <returns>
    /// Returns <c>204 No Content</c> on successful update.
    /// Returns <c>400 Bad Request</c> if input validation fails.
    /// Returns <c>404 Not Found</c> if the post category or a related object does not exist.
    /// </returns>
    [Authorize(Roles = Roles.Admin)]
    [HttpPatch("{id:long}")]
    public async Task<ActionResult> UpdatePostCategoryName([FromRoute] long id, [FromBody] UpdatePostCategoryDetailsDto dto)
    {
        // Cleaning
        dto.Name = dto.Name.Trim();
        
        // Delegate business logic (business rules + database changes)
        Result result = await postCategoryService.UpdatePostCategoryNameAsync(id, dto.Name);

        return result.IsFailure 
            ? this.MapErrorResult(result)
            : NoContent();
    }

    /// <summary>
    /// Deletes a post category.
    /// </summary>
    /// <param name="id">The identifier of the post category to delete.</param>
    /// <returns>
    /// Returns <c>204 No Content</c> on successful deletion.
    /// Returns <c>400 Bad Request</c> if input validation fails.
    /// Returns <c>404 Not Found</c> if the post category or a related object does not exist.
    /// </returns>
    [Authorize(Roles = Roles.Admin)]
    [HttpDelete("{id:long}")]
    public async Task<ActionResult> DeletePostCategory([FromRoute] long id)
    {
        // Delegate business logic (business rules + database changes)
        Result result = await postCategoryService.DeletePostCategoryAsync(id);

        return result.IsFailure 
            ? this.MapErrorResult(result)
            : NoContent();
    }
}