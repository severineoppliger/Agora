using Agora.Core.Models;
using Agora.Core.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agora.Infrastructure.Configuration;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("Transactions");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Price)
            .IsRequired();
        builder.ToTable( t =>
        {
            t.HasCheckConstraint("CK_Transaction_Price_Range",
                $"Price >= {ValidationRules.Transaction.PriceMin} AND Price <= {ValidationRules.Transaction.PriceMax}");
        });

        builder.HasOne(t => t.Post)
            .WithMany(p=>p.Transactions)
            .HasForeignKey(t => t.PostId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.TransactionStatus)
            .WithMany(t => t.Transactions)
            .HasForeignKey(t => t.TransactionStatusId);

        builder.HasOne(t => t.Buyer)
            .WithMany(u => u.TransactionsAsBuyer)
            .HasForeignKey(t => t.BuyerId);
        
        builder.HasOne(t => t.Seller)
            .WithMany(u => u.TransactionsAsSeller)
            .HasForeignKey(t => t.SellerId);
        
        builder.Property(t => t.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
        
        builder.Property(t => t.TransactionDate)
            .HasColumnType("date");
        
        builder.Property(t => t.CompletedAt)
            .IsRequired(false)
            .HasDefaultValue(null);
    }
}