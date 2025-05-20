using Agora.Core.Models;
using Agora.Infrastructure.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Agora.Infrastructure.Data;

public class AgoraDbContext(DbContextOptions options) : IdentityDbContext<AppUser>(options)
{
    public DbSet<Post> Posts { get; set; }
    public DbSet<PostCategory> PostCategories { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<TransactionStatus> TransactionStatus { get; set; }
    
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



