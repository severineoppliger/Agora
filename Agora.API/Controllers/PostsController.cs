using System.Security.Authentication;
using Agora.API.DTOs.Post;
using Agora.API.Extensions;
using Agora.API.InputValidation;
using Agora.API.InputValidation.Interfaces;
using Agora.API.QueryParams;
using Agora.Core.Common;
using Agora.Core.Constants;
using Agora.Core.Enums;
using Agora.Core.Interfaces;
using Agora.Core.Interfaces.BusinessServices;
using Agora.Core.Models;
using Agora.Core.Models.Filters;
using Agora.Core.Models.Requests;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Agora.API.Controllers;

/// <summary>
/// Handles creation, updating, deletion, and retrieval of posts offered or requested by users.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class PostsController(
    IPostService postService,
    IMapper mapper,
    IInputValidator inputValidator,
    IUserContextService userContextService)
    : ControllerBase
{
    private const string EntityName = "post";
    
    /// <summary>
    /// Retrieves all public posts visible in the catalog (i.e., with status "Active"),
    /// optionally filtered and sorted using query parameters.
    /// </summary>
    /// <param name="queryParameters">Optional filtering parameters such as title, price range, category, etc.</param>
    /// <returns>Returns <c>200 OK</c> with a list of summarized post information available to all users.</returns>
    [HttpGet("catalog")]
    public async Task<ActionResult<IReadOnlyList<PostSummaryDto>>> GetPostsCatalogue([FromQuery] PostQueryParameters queryParameters)
    {
        // Delegate business logic
        PostFilter internalPostFilter = mapper.Map<PostFilter>(queryParameters);

        Result<IReadOnlyList<Post>> result = await postService.GetAllPostsAsync(
            PostVisibilityMode.CatalogOnly,
            internalPostFilter, 
            null);
        IReadOnlyList<Post> posts = result.Value!;
        
        return Ok(mapper.Map<IReadOnlyList<PostSummaryDto>>(posts));
    }
    
    /// <summary>
    /// Retrieves all posts created by the currently authenticated user.
    /// Includes posts with status "Active" or "Inactive", but not "Deleted".
    /// </summary>
    /// <param name="queryParameters">Optional filtering parameters such as title, price range, category, etc.</param>
    /// <returns>
    /// Returns <c>200 OK</c> with a list of summarized post information owned by the current user.
    /// Returns <c>401 Unauthorized</c> if the user is not authenticated.
    /// </returns>
    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<IReadOnlyList<PostSummaryDto>>> GetCurrentUserPosts([FromQuery] PostQueryParameters queryParameters)
    {
        // Extract current user's context from claims
        UserContext userContext;
        try
        {
            userContext = userContextService.GetCurrentUserContext();
        }
        catch (AuthenticationException ex)
        {
            return Unauthorized(ex.Message);
        }
        
        // Delegate business logic
        PostFilter internalPostFilter = mapper.Map<PostFilter>(queryParameters);

        Result<IReadOnlyList<Post>> result = await postService.GetAllPostsAsync(
            PostVisibilityMode.UserOwnPosts,
            internalPostFilter, 
            userContext);
        IReadOnlyList<Post> posts = result.Value!;
        
        return Ok(mapper.Map<IReadOnlyList<PostSummaryDto>>(posts));
    }
    
    /// <summary>
    /// Retrieves all posts from all users, regardless of their status.
    /// This action is restricted to administrators only.
    /// </summary>
    /// <param name="queryParameters">Optional filtering parameters such as title, price range, category, user, etc.</param>
    /// <returns>
    /// Returns <c>200 OK</c> with a list of summarized post information across the platform.
    /// Returns <c>401 Unauthorized</c> if the user is not authenticated.
    /// Returns <c>403 Forbidden</c> if the user does not have admin privileges.
    /// </returns>
    [Authorize(Roles = Roles.Admin)]
    [HttpGet("all")]
    public async Task<ActionResult<IReadOnlyList<PostSummaryDto>>> GetAllPosts([FromQuery] PostQueryParameters queryParameters)
    {
        // Extract current user's context from claims
        UserContext userContext;
        try
        {
            userContext = userContextService.GetCurrentUserContext();
        }
        catch (AuthenticationException ex)
        {
            return Unauthorized(ex.Message);
        }
        
        // Delegate business logic
        PostFilter internalPostFilter = mapper.Map<PostFilter>(queryParameters);

        Result<IReadOnlyList<Post>> result = await postService.GetAllPostsAsync(
            PostVisibilityMode.AdminView,
            internalPostFilter, 
            userContext);
        IReadOnlyList<Post> posts = result.Value!;
        
        return Ok(mapper.Map<IReadOnlyList<PostSummaryDto>>(posts));
    }
    
    /// <summary>
    /// Retrieves detailed information of a specific post by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the post to retrieve.</param>
    /// <returns>
    /// Returns <c>200 OK</c> with the post details if found and authorized.
    /// Returns <c>401 Unauthorized</c> if the user is not allowed to view the post.
    /// Returns <c>404 Not Found</c> if the post does not exist.
    /// </returns>
    [HttpGet("{id:long}")]
    public async Task<ActionResult<PostDetailsDto>> GetPost([FromRoute] long id)
    {
        // Delegate business logic depending on if the user is authenticated or not
        Result<Post> result;
        if (!userContextService.IsAuthenticated())
        {
            result = await postService.GetPostByIdAsync(id, null);
        }
        else
        {
            // Extract current user's context from claims
            UserContext userContext;
            try
            {
                userContext = userContextService.GetCurrentUserContext();
            }
            catch (AuthenticationException ex)
            {
                return Unauthorized(ex.Message);
            }
            result = await postService.GetPostByIdAsync(id, userContext);
        }
        
        if (result.IsFailure)
        {
            return this.MapErrorResult(result);
        }
        
        // Return transaction if no error
        Post? post = result.Value;
        return post == null 
            ? NotFound(ErrorMessages.NotFound(EntityName))
            : Ok(mapper.Map<PostDetailsDto>(post));
    }
    
    /// <summary>
    /// Creates a new post.
    /// </summary>
    /// <param name="dto">The post data transfer object containing creation details.</param>
    /// <returns>
    /// Returns <c>201 Created</c> with the newly created post details.
    /// Returns <c>400 Bad Request</c> if input or business rules validation fails.
    /// Returns <c>401 Unauthorized</c> if the user is not authenticated.
    /// </returns>
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<PostDetailsDto>> CreatePost([FromBody] CreatePostDto dto)
    {
        // Validate input DTO
        InputValidationResult inputValidationResult = await inputValidator.ValidateCreatePostDtoAsync(dto);
        if (!inputValidationResult.IsValid)
        {
            return BadRequest(inputValidationResult.Errors);
        }

        // Assign userId of current user to the post
        // Extract current user's context from claims
        UserContext userContext;
        try
        {
            userContext = userContextService.GetCurrentUserContext();
        }
        catch (AuthenticationException ex)
        {
            return Unauthorized(ex.Message);
        }

        // Map the DTO to the full entity and delegate business logic (business rules + database changes)
        Post post = mapper.Map<Post>(dto);
    
        Result<Post> result = await postService.CreatePostAsync(post, userContext);

        if (result.IsFailure)
        {
            return this.MapErrorResult(result);
        }
        
        // Treat success case
        PostDetailsDto createdPostDetailsDto = mapper.Map<PostDetailsDto>(result.Value);
        return CreatedAtAction(nameof(GetPost), new { id = result.Value!.Id }, createdPostDetailsDto);
    }
    
    /// <summary>
    /// Updates details of an existing post partially.
    /// Only allowed if the current user has modification rights.
    /// </summary>
    /// <param name="id">The identifier of the post to update.</param>
    /// <param name="dto">Partial post data to update.</param>
    /// <returns>
    /// Returns <c>204 No Content</c> on successful update.
    /// Returns <c>400 Bad Request</c> if input validation fails.
    /// Returns <c>401 Unauthorized</c> if the user is not authenticated.
    /// Returns <c>403 Forbidden</c> if the user has not the modification rights for this post.
    /// Returns <c>404 Not Found</c> if the post or a related object does not exist.
    /// </returns>
    [Authorize]
    [HttpPatch("{id:long}")]
    public async Task<ActionResult> UpdatePostDetails([FromRoute] long id, [FromBody] UpdatePostDetailsDto dto)
    {
        // Validate input DTO
        InputValidationResult inputValidationResult = await inputValidator.ValidateUpdatePostDtoAsync(dto);
        if (!inputValidationResult.IsValid)
        {
            return BadRequest(inputValidationResult.Errors);
        }
        
        // Extract current user's context from claims
        UserContext userContext;
        try
        {
            userContext = userContextService.GetCurrentUserContext();
        }
        catch (AuthenticationException ex)
        {
            return Unauthorized(ex.Message);
        }
        
        // Delegate business logic (business rules + database changes)
        PostDetailsUpdate newDetails = mapper.Map<PostDetailsUpdate>(dto);
        Result result = await postService.UpdatePostDetailsAsync(id, newDetails, userContext);

        return result.IsFailure 
            ? this.MapErrorResult(result)
            : NoContent();
    }

    /// <summary>
    /// Make a post public by setting its status to <c>Active</c>.
    /// </summary>
    /// <param name="id">The unique identifier of the post to make public.</param>
    /// <returns>
    /// Returns <c>204 No Content</c> if the operation succeeds.
    /// Returns <c>401 Unauthorized</c> if the user is not authenticated.
    /// Returns <c>403 Forbidden</c> if the user does not have permission to activate this post.
    /// Returns <c>404 Not Found</c> if the post does not exist or is not visible to the user.
    /// Returns <c>400 Bad Request</c> if the action violates business rules.
    /// </returns>
    [Authorize]
    [HttpPut("{id:long}/activate")]
    public async Task<IActionResult> ActivatePost([FromRoute] long id)
    {
        // Extract current user's context from claims
        UserContext userContext;
        try
        {
            userContext = userContextService.GetCurrentUserContext();
        }
        catch (AuthenticationException ex)
        {
            return Unauthorized(ex.Message);
        }
        
        // Delegate business logic (business rules + database changes)
        Result result = await postService.ChangePostStatusAsync(id, userContext, PostStatus.Active);
        return result.IsFailure 
            ? this.MapErrorResult(result)
            : NoContent();
    }

    /// <summary>
    /// Hide a post from the public by setting its status to <c>Inactive</c>.
    /// </summary>
    /// <param name="id">The unique identifier of the post to hide.</param>
    /// <returns>
    /// Returns <c>204 No Content</c> if the operation succeeds.
    /// Returns <c>401 Unauthorized</c> if the user is not authenticated.
    /// Returns <c>403 Forbidden</c> if the user does not have permission to deactivate this post.
    /// Returns <c>404 Not Found</c> if the post does not exist or is not visible to the user.
    /// Returns <c>400 Bad Request</c> if the action violates business rules.
    /// </returns>
    [Authorize]
    [HttpPut("{id:long}/deactivate")]
    public async Task<IActionResult> DeactivatePost([FromRoute] long id)
    {
        // Extract current user's context from claims
        UserContext userContext;
        try
        {
            userContext = userContextService.GetCurrentUserContext();
        }
        catch (AuthenticationException ex)
        {
            return Unauthorized(ex.Message);
        }
        
        // Delegate business logic (business rules + database changes)
        Result result = await postService.ChangePostStatusAsync(id, userContext, PostStatus.Inactive);
        return result.IsFailure 
            ? this.MapErrorResult(result)
            : NoContent();
    }
    
    /// <summary>
    /// Deletes a post.
    /// </summary>
    /// <param name="id">The identifier of the post to delete.</param>
    /// <returns>
    /// Returns <c>204 No Content</c> on successful deletion.
    /// Returns <c>400 Bad Request</c> if input validation fails.
    /// Returns <c>404 Not Found</c> if the post or a related object does not exist.
    /// </returns>
    [Authorize]
    [HttpDelete("{id:long}")]
    public async Task<ActionResult> DeletePost([FromRoute] long id)
    {
        // Extract current user's context from claims
        UserContext userContext;
        try
        {
            userContext = userContextService.GetCurrentUserContext();
        }
        catch (AuthenticationException ex)
        {
            return Unauthorized(ex.Message);
        }
        
        // Delegate business logic (business rules + database changes)
        Result result = await postService.DeletePostAsync(id, userContext);

        return result.IsFailure 
            ? this.MapErrorResult(result)
            : NoContent();
        
    }
}