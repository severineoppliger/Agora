using Agora.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agora.Infrastructure.Configuration;

public class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.ToTable("Posts");

        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.Title).IsRequired().HasMaxLength(100);
        
        builder.Property(p => p.Description).IsRequired().HasMaxLength(2000);
        
        builder.Property(p => p.Price).IsRequired();
        
        builder.Property(p => p.Type).IsRequired();
        
        builder.Property(p => p.Status).IsRequired();

        builder.HasOne(p => p.PostCategory)
            .WithMany(p => p.Posts)
            .HasForeignKey(p => p.PostCategoryId);
        
        builder.HasOne(p => p.User)
            .WithMany(u => u.Posts)
            .HasForeignKey(p=> p.UserId);

        builder.HasMany(p => p.Transactions)
            .WithOne(t => t.Post)
            .HasForeignKey(t => t.PostId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Property(p => p.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
        
        builder.Property(p => p.UpdatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP");
    }
}