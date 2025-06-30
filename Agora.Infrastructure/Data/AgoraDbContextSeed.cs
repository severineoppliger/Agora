using Agora.Core.Constants;
using Agora.Core.Enums;
using Agora.Core.Extensions;
using Agora.Core.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Agora.Infrastructure.Data;

/// <summary>
/// Provides static methods to seed initial and development data into the Agora database context.
/// Includes methods for seeding post categories, transaction statuses, roles, users, posts, and transactions.
/// </summary>
public static class AgoraDbContextSeed
{
    #region Static Seeding
    
    /// <summary>
    /// Seeds predefined post categories into the model builder for migrations.
    /// </summary>
    /// <param name="modelBuilder">The model builder to configure entity data.</param>
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

    /// <summary>
    /// Seeds predefined transaction statuses into the model builder for migrations.
    /// </summary>
    /// <param name="modelBuilder">The model builder to configure entity data.</param>
    internal static void SeedTransactionStatus(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TransactionStatus>().HasData(
            new TransactionStatus
            {
                Id = 1, Name = "En attente",
                Description =
                    "Une demande d'échange a été initiée par un des deux utilisateurs mais n'est pas encore acceptée par l'autre utilisateur.",
                IsFinal = false, 
                IsSuccess = false,
                EnumValue = TransactionStatusEnum.Pending
            },
            new TransactionStatus
            {
                Id = 2, Name = "Acceptée",
                Description =
                    "La demande d'échange a été acceptée par l'autre utilisateur. La transaction peut avoir lieu.",
                IsFinal = false, 
                IsSuccess = false,
                EnumValue = TransactionStatusEnum.Accepted
            },
            new TransactionStatus
            {
                Id = 3, Name = "Refusée",
                Description =
                    "La demande d'échange a été refusée par l'autre utilisateur. La transaction n'aura donc pas lieu.",
                IsFinal = true,
                IsSuccess = false,
                EnumValue = TransactionStatusEnum.Refused
            },
            new TransactionStatus
            {
                Id = 4, Name = "Annulée",
                Description = "La demande d'échange a été annulée par l’initiateur avant acceptation de l'autre partie.", 
                IsFinal = true,
                IsSuccess = false,
                EnumValue = TransactionStatusEnum.Cancelled
            },
            new TransactionStatus
            {
                Id = 5, Name = "Échouée",
                Description = "Le service n'a pas pu être réalisé, malgré la confirmation de la demande d'échange.",
                IsFinal = true, 
                IsSuccess = false,
                EnumValue = TransactionStatusEnum.Failed
            },
            new TransactionStatus
            {
                Id = 6, Name = "Partiellement validée",
                Description =
                    "Le service a été réalisé et validé par un seul utilisateur, en attente de confirmation de l'autre.",
                IsFinal = false,
                IsSuccess = false,
                EnumValue = TransactionStatusEnum.PartiallyValidated
            },
            new TransactionStatus
            {
                Id = 7, Name = "Terminée",
                Description =
                    "Le service a été réalisé et la transaction a été validée soit par les deux parties," +
                    "soit uniquement par l'une d'entre elles si un délai de validation automatique s'est écoulé" +
                    "(par exemple X jours) sans objection de l'autre partie. Les points sont transférés de l'acheteur au vendeur.",
                IsFinal = true, 
                IsSuccess = true,
                EnumValue = TransactionStatusEnum.Completed
            },
            new TransactionStatus
            {
                Id = 8, Name = "En litige",
                Description = "Un désaccord a été signalé par l'une des parties concernant le déroulement de la transaction après qu'un accord initial ait été confirmé par les deux participants.",
                IsFinal = false, 
                IsSuccess = false,
                EnumValue = TransactionStatusEnum.InDispute
            },
            new TransactionStatus
            {
                Id = 9, Name = "Résolue et acceptée",
                Description = "La résolution du litige s'est soldé par la validation de la transaction.",
                IsFinal = true,
                IsSuccess = true,
                EnumValue = TransactionStatusEnum.ResolvedAccepted
            },
            new TransactionStatus
            {
                Id = 10, Name = "Résolue et annulée",
                Description = "La résolution du litige s'est soldé par l'annulation de la transaction.", 
                IsFinal = true,
                IsSuccess = false,
                EnumValue = TransactionStatusEnum.ResolvedCancelled
            }
        );
    }
    #endregion

    // Dynamic seeding - Done at program execution
    //      For Roles: in any environnement
    //      For Users, Posts and Transactions: only in development environment
    #region Dynamic Seeding
    
    /// <summary>
    /// Seeds roles asynchronously, ensuring required roles exist (e.g., Admin).
    /// </summary>
    /// <param name="roleManager">The role manager to manage identity roles.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
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

    /// <summary>
    /// Seeds development data including users, posts, and transactions if the database is empty.
    /// Intended for development environment only.
    /// </summary>
    /// <param name="context">The Agora database context.</param>
    /// <param name="userManager">The user manager to create users.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static async Task SeedDevelopmentDataAsync(AgoraDbContext context, UserManager<User> userManager)
    {
        if (!context.Users.Any())
        {
            await SeedUsers(userManager);
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

    /// <summary>
    /// Seeds predefined users asynchronously, including an admin and test members.
    /// Throws exceptions on invalid IDs or creation failures.
    /// </summary>
    /// <param name="userManager">The user manager to create and manage users.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    private static async Task SeedUsers(UserManager<User> userManager)
    {
        var users = new List<(string Id, string UserName, string Email, string Password, int Credit, bool IsAdmin)>
        {
            ("00000000-0000-0000-0000-000000000001", "admin", "admin@test.ch", "Admin1234$", 100, true),
            ("00000000-0000-0000-0000-000000000002", "testmember1", "testmember1@test.ch", "Test1234!", 200, false),
            ("00000000-0000-0000-0000-000000000003", "testmember2", "testmember2@test.ch", "Test2345!", 300, false),
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

            var user = new User
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

    /// <summary>
    /// Seeds predefined posts asynchronously if none exist in the database.
    /// </summary>
    /// <param name="context">The Agora database context.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
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
                    Status = PostStatus.Active,
                    PostCategoryId = 1,
                    OwnerUserId = "00000000-0000-0000-0000-000000000001",
                    CreatedAt = DateTime.UtcNow
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
                    CreatedAt = DateTime.UtcNow
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
                    CreatedAt = DateTime.UtcNow
                }

            ];

            await context.Posts.AddRangeAsync(posts);
            await context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Seeds predefined transactions asynchronously if none exist in the database.
    /// </summary>
    /// <param name="context">The Agora database context.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    private static async Task SeedTransactions(AgoraDbContext context)
    {
        if (!context.Transactions.Any())
        {
            List<Transaction> transactions =
            [
                new Transaction
                {
                    Title = "Test transaction between Member 1 and Admin",
                    Price = 10,
                    PostId = 1,
                    TransactionStatusId = 1,
                    InitiatorId = "00000000-0000-0000-0000-000000000002",
                    BuyerId = "00000000-0000-0000-0000-000000000002",
                    BuyerConfirmed = false,
                    SellerId = "00000000-0000-0000-0000-000000000001",
                    SellerConfirmed = false,
                    CreatedAt = DateTime.UtcNow
                }
            ];

            await context.Transactions.AddRangeAsync(transactions);
            await context.SaveChangesAsync();
        }
    }
    
    #endregion
}