using Agora.Core.Models;
using Agora.Core.Models.Entities;
using Agora.Core.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agora.Infrastructure.Configuration;

public class TransactionStatusConfiguration : IEntityTypeConfiguration<TransactionStatus>
{
    public void Configure(EntityTypeBuilder<TransactionStatus> builder)
    {
        builder.ToTable("TransactionsStatus");

        builder.HasKey(t => t.Id);
        
        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(ValidationConstants.TransactionStatus.NameMaxLength);
        builder.HasIndex(t => t.Name)
            .IsUnique();
        
        builder.Property(t => t.Description)
            .IsRequired()
            .HasMaxLength(ValidationConstants.TransactionStatus.DescriptionMaxLength);
        
        builder.Property(t => t.IsFinal)
            .IsRequired();
        
        builder.Property(t => t.IsSuccess)
            .IsRequired();
        
        builder.Property(p => p.EnumValue)
            .HasConversion<string>()
            .IsRequired();
        
        builder.HasMany(t => t.Transactions)
            .WithOne(t => t.TransactionStatus)
            .HasForeignKey(t => t.TransactionStatusId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}