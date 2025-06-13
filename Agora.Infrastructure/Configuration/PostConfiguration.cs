using Agora.Core.Models;
using Agora.Core.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agora.Infrastructure.Configuration;

public class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.ToTable("Posts");

        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.Title)
            .IsRequired()
            .HasMaxLength(ValidationRules.Post.TitleMaxLength);
        
        builder.Property(p => p.Description)
            .IsRequired()
            .HasMaxLength(ValidationRules.Post.DescriptionMaxLength);
        
        builder.Property(p => p.Price)
            .IsRequired();
        builder.ToTable( t =>
        {
            t.HasCheckConstraint("CK_Post_Price_Range",
                $"Price >= {ValidationRules.Post.PriceMin} AND Price <= {ValidationRules.Post.PriceMax}");
        });
        
        builder.Property(p => p.Type)
            .HasConversion<string>() // The enum is stored as a string in DB
            .IsRequired();
        
        builder.Property(p => p.Status)
            .HasConversion<string>()
            .IsRequired();

        builder.HasOne(p => p.PostCategory)
            .WithMany(p => p.Posts)
            .HasForeignKey(p => p.PostCategoryId);
        
        builder.HasOne(p => p.Owner)
            .WithMany(u => u.Posts)
            .HasForeignKey(p=> p.OwnerUserId);

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