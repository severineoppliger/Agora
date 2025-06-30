using Agora.Core.Models.Entities;
using Agora.Core.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agora.Infrastructure.Configuration;

/// <summary>
/// Configuration for the <see cref="PostCategory"/> entity for the <b>PostCategories</b> DB table
/// including table mapping, keys, property constraints, indexes,
/// and relationships with <see cref="Post"/> entities.
/// </summary>
public class PostCategoryConfiguration : IEntityTypeConfiguration<PostCategory>
{
    public void Configure(EntityTypeBuilder<PostCategory> builder)
    {
        builder.ToTable("PostCategories");

        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(ValidationConstants.PostCategory.NameMaxLength);
        builder.HasIndex(p => p.Name)
            .IsUnique();
        
        builder.HasMany(p => p.Posts)
            .WithOne(p => p.PostCategory)
            .HasForeignKey(p => p.PostCategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}