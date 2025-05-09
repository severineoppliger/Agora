using Agora.Core.Enums;
using Agora.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Agora.Infrastructure.Data;

public class AgoraDbContextSeed
{
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
                    "La demande d'échange a été initiée par un des deux utilisateurs mais pas encore acceptée par l'autre utilisateur.",
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
                Description = "La demande d'échange a été annulée par l’un des deux utilisateurs.", IsFinal = false,
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
                Id = 6, Name = "En cours", Description = "Le service est en train d’être réalisé.", IsFinal = false,
                IsSuccess = false
            },
            new TransactionStatus
            {
                Id = 7, Name = "Partiellement validée",
                Description =
                    "Le service a été réalisé et validé par un seul utilisateur, en attente de confirmation de l'autre.",
                IsFinal = false, IsSuccess = false
            },
            new TransactionStatus
            {
                Id = 8, Name = "Terminée",
                Description =
                    "Le service a été effectué et validé par les deux parties. Les points sont transférés de l'acheteur au vendeur.",
                IsFinal = true, IsSuccess = true
            },
            new TransactionStatus
            {
                Id = 9, Name = "En litige",
                Description = "Le service a été effectué mais un désaccord a été signalé sur la transaction.",
                IsFinal = false, IsSuccess = false
            },
            new TransactionStatus
            {
                Id = 10, Name = "Résolue et acceptée",
                Description =
                    "Le litige a été résolu et les valeurs actuelles de la transaction ont été acceptées par les deux partis.",
                IsFinal = true, IsSuccess = true
            },
            new TransactionStatus
            {
                Id = 11, Name = "Résolue et annulée",
                Description = "La résolution du litige s'est soldé par l'annulation de la transaction.", IsFinal = true,
                IsSuccess = false
            }
        );
    }

    public static async Task SeedDevelopmentDataAsync(AgoraDbContext context)
    {
        if (!context.Users.Any())
        {
            await SeedUsers(context);
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


    private static async Task SeedUsers(AgoraDbContext context)
    {
        if (!context.Users.Any())
        {
            List<User> users =
            [
                new User
                {
                    Id = 1,
                    Username = "admin",
                    Email = "admin@test.ch",
                    PasswordHash = "Admin1234$", // TODO Hash le mot de passe
                    CreatedAt = DateTime.Now,
                    Credit = 100
                },

                new User
                {
                    Id = 2,
                    Username = "test1",
                    Email = "test1@test.ch",
                    PasswordHash = "Test1", // TODO Hash le mot de passe
                    CreatedAt = DateTime.Now,
                    Credit = 200
                },

                new User
                {
                    Id = 3,
                    Username = "test2",
                    Email = "test2@test.ch",
                    PasswordHash = "Test2", // TODO Hash le mot de passe
                    CreatedAt = DateTime.Now,
                    Credit = 300
                }
            ];

            await context.Users.AddRangeAsync(users);
            await context.SaveChangesAsync();
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
                    Status = PostStatus.InTransaction,
                    PostCategoryId = 1,
                    UserId = 1,
                    CreatedAt = DateTime.Now
                },

                new Post
                {
                    Title = "Annonce 2 de l'utilisateur 2 - Covoiturage",
                    Description = "Demande de covoiturage",
                    Price = 20,
                    Type = PostType.Request,
                    Status = PostStatus.Draft,
                    PostCategoryId = 2,
                    UserId = 2,
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
                    UserId = 3,
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
                    BuyerId = 2,
                    SellerId = 1,
                    CreatedAt = DateTime.Now
                }
            ];

            await context.Transactions.AddRangeAsync(transactions);
            await context.SaveChangesAsync();
        }
    }
}