using Agora.Core.Models;
using Agora.Infrastructure.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Agora.Infrastructure.Data;

public class AgoraDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<PostCategory> PostCategories { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<TransactionStatus> TransactionStatus { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new PostConfiguration());
        modelBuilder.ApplyConfiguration(new PostCategoryConfiguration());
        modelBuilder.ApplyConfiguration(new TransactionConfiguration());
        modelBuilder.ApplyConfiguration(new TransactionStatusConfiguration());
    }
    
};



