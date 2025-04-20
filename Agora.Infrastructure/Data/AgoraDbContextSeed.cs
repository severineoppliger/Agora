using Agora.Core.Enums;
using Agora.Core.Models;

namespace Agora.Infrastructure.Data;

public class AgoraDbContextSeed
{
    public static async Task SeedAsync(AgoraDbContext context)
    {
        await SeedPosts(context);
        await SeedUsers(context);
    }

    private static async Task SeedPosts(AgoraDbContext context)
    {
        /*if (!context.Posts.Any())
        {
            var posts = new List<Post>
            {
                new Post
                {
                    Title = "Post 1 - Testing purpose",
                    Description = "Post 1 - Description",
                    Price = 10,
                    Type = PostType.Offer,
                    Status = PostStatus.Active,
                    PostCategoryId = 1,
                    UserId = 1,
                },
                new Post
                {
                    Title = "Post 2 - Testing purpose",
                    Description = "Post 2 - Description",
                    Price = 20,
                    Type = PostType.Request,
                    Status = PostStatus.Draft,
                    PostCategoryId = 2,
                    UserId = 2,
                }
            };

            context.Posts.AddRange(posts);
            await context.SaveChangesAsync();
        }*/
    }

    private static async Task SeedUsers(AgoraDbContext context)
    {
        
    }
}