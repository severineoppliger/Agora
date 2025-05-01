using Agora.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agora.Infrastructure.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);
        
        builder.Property(u => u.Username)
            .IsRequired()
            .HasMaxLength(50);
        builder.HasIndex(u => u.Username).IsUnique();

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(255);
        builder.HasIndex(u => u.Email).IsUnique();

        builder.Property(u => u.PasswordHash)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(u => u.Credit)
            .IsRequired();
        
        builder.HasMany(u => u.Posts)
            .WithOne(p => p.User)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.TransactionsAsBuyer)
            .WithOne(t => t.Buyer)
            .HasForeignKey(t => t.BuyerId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(u => u.TransactionsAsSeller)
            .WithOne(t => t.Seller)
            .HasForeignKey(t => t.SellerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(u => u.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(u => u.LastLoginAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
    }
}