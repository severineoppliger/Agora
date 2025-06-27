using Agora.Core.Models.Entities;
using Agora.Infrastructure.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Agora.Infrastructure.Data;

/// <summary>
/// Represents the database context for the Agora application, 
/// inheriting from IdentityDbContext to include user identity management.
/// Provides access to the main entity sets (tables) and applies configurations and seed data.
/// </summary>
public class AgoraDbContext(DbContextOptions options) : IdentityDbContext<User>(options)
{
    /// <summary>
    /// Gets or sets the <see cref="DbSet{Post}"/> representing the <b>Posts</b> table in the database.
    /// </summary>
    public DbSet<Post> Posts { get; set; }
    
    /// <summary>
    /// Gets or sets the <see cref="DbSet{PostCategory}"/> representing the <b>PostCategories</b> table in the database.
    /// </summary>
    public DbSet<PostCategory> PostCategories { get; set; }
    
    /// <summary>
    /// Gets or sets the <see cref="DbSet{Transaction}"/> representing the <b>Transactions</b> table in the database.
    /// </summary>
    public DbSet<Transaction> Transactions { get; set; }
    
    /// <summary>
    /// Gets or sets the <see cref="DbSet{TransactionStatus}"/> representing the <b>TransactionStatus</b> table in the database.
    /// </summary>
    public DbSet<TransactionStatus> TransactionStatus { get; set; }
    
    /// <summary>
    /// Configures the entity model for the context by applying entity configurations 
    /// (which define how entities map to database tables and their relationships) 
    /// and seeds initial data.
    /// </summary>
    /// <param name="modelBuilder">The builder used to construct the model for this context.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new PostConfiguration());
        modelBuilder.ApplyConfiguration(new PostCategoryConfiguration());
        modelBuilder.ApplyConfiguration(new TransactionConfiguration());
        modelBuilder.ApplyConfiguration(new TransactionStatusConfiguration());
        
        AgoraDbContextSeed.SeedPostCategories(modelBuilder);
        AgoraDbContextSeed.SeedTransactionStatus(modelBuilder);
    }
    
};



