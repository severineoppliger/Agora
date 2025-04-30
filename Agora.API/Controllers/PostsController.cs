using Agora.API.DTOs.Post;
using Agora.Core.Interfaces;
using Agora.Core.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Agora.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostsController(IPostRepository repo, IMapper mapper) : ControllerBase
{
    private const string PostNotFoundMessage = "Post not found.";

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<PostSummaryDto>>> GetAllPosts()
    {
        IReadOnlyList<Post> posts = await repo.GetAllPostsAsync();
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

    [HttpPost]
    public async Task<ActionResult<PostDetailsDto>> CreatePost([FromBody] CreatePostDto postDto)
    {
        Post post = mapper.Map<Post>(postDto);
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

    [HttpPut("{id:long}")]
    public async Task<ActionResult> UpdatePost([FromRoute] long id, [FromBody] UpdatePostDto postDto)
    {
        Post? existingPost = await repo.GetPostByIdAsync(id);

        if (existingPost == null) return NotFound(PostNotFoundMessage);
        
        // Apply the updated fields exposed in the DTO to the existing post
        mapper.Map(postDto, existingPost); 

        return await repo.SaveChangesAsync() ? NoContent() : BadRequest("Problem updating the post.");
    }

    [HttpDelete("{id:long}")]
    public async Task<ActionResult> DeletePost([FromRoute] long id)
    {
        Post? post = await repo.GetPostByIdAsync(id);

        if (post == null)
        {
            return NotFound(PostNotFoundMessage);
        }

        repo.DeletePost(post);

        return await repo.SaveChangesAsync() ? NoContent() : BadRequest("Problem deleting the post.");
    }
}