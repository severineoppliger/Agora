using System.Security.Claims;
using Agora.API.DTOs.Post;
using Agora.API.InputValidation.Interfaces;
using Agora.API.Orchestrators.Interfaces;
using Agora.API.QueryParams;
using Agora.Core.Enums;
using Agora.Core.Interfaces;
using Agora.Core.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Agora.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostsController(
    IPostRepository repo,
    IMapper mapper,
    IInputValidator inputValidator,
    IBusinessRulesValidationOrchestrator businessRulesValidationOrchestrator)
    : ControllerBase
{
    private const string PostNotFoundMessage = "Post not found.";
    private const string NotOwnerMessage = "Current user is not the owner of the post.";

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<PostSummaryDto>>> GetAllPosts([FromQuery] PostQueryParameters queryParameters)
    {
        IReadOnlyList<Post> posts = await repo.GetAllPostsAsync(queryParameters);
        return Ok(mapper.Map<IReadOnlyList<PostSummaryDto>>(posts));
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<PostDetailsDto>> GetPost([FromRoute] long id)
    {
        Post? post = await repo.GetPostByIdAsync(id);
        
        return post == null
            ? NotFound(PostNotFoundMessage)
            : Ok(mapper.Map<PostDetailsDto>(post));
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<PostDetailsDto>> CreatePost([FromBody] CreatePostDto postDto)
    {
        // Cleaning
        postDto.Title = postDto.Title.Trim();
        postDto.Description = postDto.Description.Trim();
        
        // Input validation
        List<string> inputErrors = await inputValidator.ValidateInputPostDtoAsync(postDto);
        if (inputErrors.Count != 0)
        {
            return BadRequest(new { Errors = inputErrors });
        }
        
        // Assign userId of current user to the post
        string? currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (currentUserId != null)
        {
            return Unauthorized("User ID not found in claims.");
        }

        // Transform to the full entity and validate with business rules
        Post post = mapper.Map<Post>(postDto);
        post.Status = PostStatus.Active;
        post.OwnerUserId = currentUserId!;
        
        IList<string> businessRulesErrors = await businessRulesValidationOrchestrator.ValidateAndProcessPostAsync(post);
        if (businessRulesErrors.Count != 0)
        {
            return BadRequest(new { Errors = businessRulesErrors });
        }

        // Add to database
        repo.AddPost(post);
        
        if (await repo.SaveChangesAsync())
        {
            Post? createdPost = await repo.GetPostByIdAsync(post.Id);
            
            if (createdPost == null)
            {
                return StatusCode(500, "Post was saved but could not be retrieved.");
            }
            
            PostDetailsDto createdPostDetailsDto = mapper.Map<PostDetailsDto>(createdPost);
            
            return CreatedAtAction(nameof(GetPost), new { id = createdPost.Id }, createdPostDetailsDto);
        }
        
        return BadRequest("Problem creating the post.");
    }

    [Authorize]
    [HttpPut("{id:long}")]
    public async Task<ActionResult> UpdatePost([FromRoute] long id, [FromBody] UpdatePostDto postDto)
    {
        // Cleaning
        postDto.Title = postDto.Title.Trim();
        postDto.Description = postDto.Description.Trim();
        
        // Retrieve the existing post
        Post? existingPost = await repo.GetPostByIdAsync(id);
        if (existingPost == null)
        {
            return NotFound(PostNotFoundMessage);
        }
        
        // Ownership validation
        string? currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (existingPost.OwnerUserId != currentUserId)
        {
            return Unauthorized(NotOwnerMessage);
        }
        
        // Input validation
        List<string> inputErrors = await inputValidator.ValidateInputPostDtoAsync(postDto);
        if (inputErrors.Count != 0)
        {
            return BadRequest(new { Errors = inputErrors });
        }
        
        // Transform to the full entity and validate with business rules
        Post post = mapper.Map<Post>(postDto);
        IList<string> businessRulesErrors = await businessRulesValidationOrchestrator.ValidateAndProcessPostAsync(post);
        if (businessRulesErrors.Count != 0)
        {
            return BadRequest(new { Errors = businessRulesErrors });
        }

        // Apply the updated fields exposed in the DTO to the existing post
        mapper.Map(postDto, existingPost); 

        return await repo.SaveChangesAsync()
            ? NoContent()
            : BadRequest("Problem updating the post.");
    }

    [Authorize]
    [HttpDelete("{id:long}")]
    public async Task<ActionResult> DeletePost([FromRoute] long id)
    {
        Post? post = await repo.GetPostByIdAsync(id);

        if (post == null)
        {
            return NotFound(PostNotFoundMessage);
        }
        
        // Ownership validation
        string? currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (post.OwnerUserId != currentUserId)
        {
            return Unauthorized(NotOwnerMessage);
        }

        repo.DeletePost(post);

        return await repo.SaveChangesAsync() 
            ? NoContent()
            : BadRequest("Problem deleting the post.");
    }
}