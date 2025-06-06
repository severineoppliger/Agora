using Agora.Core.Constants;
using Agora.Core.Enums;
using Agora.Core.Extensions;
using Agora.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Agora.Infrastructure.Data;

public class AgoraDbContextSeed()
{
    // Static seeding, inserted at migration
    #region staticSeeding
    internal static void SeedPostCategories(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PostCategory>().HasData(
            new PostCategory { Id = 1, Name = "Cours d'appui" },
            new PostCategory { Id = 2, Name = "Covoiturage" },
            new PostCategory { Id = 3, Name = "Informatique" },
            new PostCategory { Id = 4, Name = "Arts et culture" },
            new PostCategory { Id = 5, Name = "Démarches administratives" },
            new PostCategory { Id = 6, Name = "Déménagement" },
            new PostCategory { Id = 7, Name = "Services du quotidien" },
            new PostCategory { Id = 8, Name = "Autres" }
        );
    }

    internal static void SeedTransactionStatus(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TransactionStatus>().HasData(
            new TransactionStatus
            {
                Id = 1, Name = "En attente",
                Description =
                    "Une demande d'échange a été initiée par un des deux utilisateurs mais n'est pas encore acceptée par l'autre utilisateur.",
                IsFinal = false, IsSuccess = false
            },
            new TransactionStatus
            {
                Id = 2, Name = "Acceptée",
                Description =
                    "La demande d'échange a été acceptée par l'autre utilisateur. La transaction peut avoir lieu.",
                IsFinal = false, IsSuccess = false
            },
            new TransactionStatus
            {
                Id = 3, Name = "Refusée",
                Description =
                    "La demande d'échange a été refusée par l'autre utilisateur. La transaction n'aura donc pas lieu.",
                IsFinal = true, IsSuccess = false
            },
            new TransactionStatus
            {
                Id = 4, Name = "Annulée",
                Description = "La demande d'échange a été annulée par l’initiateur avant acceptation de l'autre partie.", 
                IsFinal = true,
                IsSuccess = false
            },
            new TransactionStatus
            {
                Id = 5, Name = "Échouée",
                Description = "Le service n'a pas pu être réalisé, malgré la confirmation de la demande d'échange.",
                IsFinal = true, IsSuccess = false
            },
            new TransactionStatus
            {
                Id = 6, Name = "Partiellement validée",
                Description =
                    "Le service a été réalisé et validé par un seul utilisateur, en attente de confirmation de l'autre.",
                IsFinal = false, IsSuccess = false
            },
            new TransactionStatus
            {
                Id = 7, Name = "Terminée",
                Description =
                    "Le service a été effectué et validé par les deux partis. Les points sont transférés de l'acheteur au vendeur.",
                IsFinal = true, IsSuccess = true
            },
            new TransactionStatus
            {
                Id = 8, Name = "En litige",
                Description = "Un désaccord a été signalé par l'une des parties concernant le déroulement de la transaction après qu'un accord initial ait été confirmé par les deux participants.",
                IsFinal = false, IsSuccess = false
            },
            new TransactionStatus
            {
                Id = 9, Name = "Résolue et acceptée",
                Description = "La résolution du litige s'est soldé par la validation de la transaction.",
                IsFinal = true, IsSuccess = true
            },
            new TransactionStatus
            {
                Id = 10, Name = "Résolue et annulée",
                Description = "La résolution du litige s'est soldé par l'annulation de la transaction.", IsFinal = true,
                IsSuccess = false
            }
        );
    }
    #endregion

    // Dynamic seeding - Done at program execution
    //      For Roles: in any environnement
    //      For Users, Posts and Transactions: only in development environment
    #region DynamicSeeding
    public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
    {
        if (!await roleManager.RoleExistsAsync(Roles.Admin))
        {
            IdentityResult roleResult = await roleManager.CreateAsync(new IdentityRole(Roles.Admin));
            if (!roleResult.Succeeded)
            {
                throw new Exception("Creation of the Admin role has failed.");
            }
        }
    }

    public static async Task SeedDevelopmentDataAsync(AgoraDbContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        if (!context.Users.Any())
        {
            await SeedUsers(userManager, roleManager);
        }
        if (!context.Posts.Any())
        {
            await SeedPosts(context);
        }
        if (!context.Transactions.Any())
        {
            await SeedTransactions(context);
        }
    }

    private static async Task SeedUsers(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        var users = new List<(string Id, string UserName, string Email, string Password, int Credit, bool IsAdmin)>
        {
            ("00000000-0000-0000-0000-000000000001", "admin", "admin@test.ch", "Admin1234$", 100, true),
            ("00000000-0000-0000-0000-000000000002", "test1", "test1@test.ch", "Test1234!", 200, false),
            ("00000000-0000-0000-0000-000000000003", "test2", "test2@test.ch", "Test2345!", 300, false),
        };

        foreach (var (id, userName, email, password, credit, isAdmin) in users)
        {
            if (!id.IsGuid())
            {
                throw new FormatException($"Invalid user ID format: {id}. Must be a valid GUID.");
            }
            
            // Check if user already exists
            if (await userManager.FindByIdAsync(id) != null)
                continue;

            var user = new AppUser
            {
                Id = id,
                UserName = userName,
                Email = email,
                CreatedAt = DateTime.UtcNow,
                Credit = credit
            };

            var result = await userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                throw new Exception($"Failed to create user {email}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }

            if (isAdmin)
            {
                await userManager.AddToRoleAsync(user, Roles.Admin);
            }
        }
    }


    private static async Task SeedPosts(AgoraDbContext context)
    {
        if (!context.Posts.Any())
        {
            List<Post> posts =
            [
                new Post
                {
                    Title = "Annonce 1 de l'utilisateur 1 - Cours d'appui",
                    Description = "Offre pour un cours d'appui - Annonce avec transaction en cours",
                    Price = 10,
                    Type = PostType.Offer,
                    Status = PostStatus.InTransactionActive,
                    PostCategoryId = 1,
                    OwnerUserId = "00000000-0000-0000-0000-000000000001",
                    CreatedAt = DateTime.Now
                },
                new Post
                {
                    Title = "Annonce 2 de l'utilisateur 2 - Covoiturage",
                    Description = "Demande de covoiturage",
                    Price = 20,
                    Type = PostType.Request,
                    Status = PostStatus.Inactive,
                    PostCategoryId = 2,
                    OwnerUserId = "00000000-0000-0000-0000-000000000002",
                    CreatedAt = DateTime.Now
                },
                new Post
                {
                    Title = "Annonce 3 de l'utilisateur 3 - Informatique",
                    Description = "Offre d'aide d'informatique",
                    Price = 30,
                    Type = PostType.Offer,
                    Status = PostStatus.Active,
                    PostCategoryId = 3,
                    OwnerUserId = "00000000-0000-0000-0000-000000000003",
                    CreatedAt = DateTime.Now
                }

            ];

            await context.Posts.AddRangeAsync(posts);
            await context.SaveChangesAsync();
        }
    }

    private static async Task SeedTransactions(AgoraDbContext context)
    {
        if (!context.Transactions.Any())
        {
            List<Transaction> transactions =
            [
                new Transaction
                {
                    Price = 10,
                    PostId = 1,
                    TransactionStatusId = 1,
                    BuyerId = "00000000-0000-0000-0000-000000000002",
                    SellerId = "00000000-0000-0000-0000-000000000001",
                    CreatedAt = DateTime.Now
                }
            ];

            await context.Transactions.AddRangeAsync(transactions);
            await context.SaveChangesAsync();
        }
    }
    
    #endregion
}