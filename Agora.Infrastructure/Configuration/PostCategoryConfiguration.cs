using Agora.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agora.Infrastructure.Configuration;

public class PostCategoryConfiguration : IEntityTypeConfiguration<PostCategory>
{
    public void Configure(EntityTypeBuilder<PostCategory> builder)
    {
        builder.ToTable("PostCategories");

        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.Name).IsRequired().HasMaxLength(255);
        builder.HasIndex(p => p.Name).IsUnique();
        
        builder.HasMany(p => p.Posts)
            .WithOne(p => p.PostCategory)
            .HasForeignKey(p => p.PostCategoryId);
    }
}