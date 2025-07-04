﻿using Agora.Core.Models.Entities;
using Agora.Core.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agora.Infrastructure.Configuration;

/// <summary>
/// Configuration for the <see cref="Transaction"/> entity for the <b>Transactions</b> DB table,
/// including table mapping, primary key, property constraints,
/// check constraints, foreign keys, and navigation properties.
/// </summary>
public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("Transactions");

        builder.HasKey(t => t.Id);
        
        builder.Property(p => p.Title)
            .IsRequired()
            .HasMaxLength(ValidationConstants.Transaction.TitleMaxLength);

        builder.Property(t => t.Price)
            .IsRequired();
        builder.ToTable( t =>
        {
            t.HasCheckConstraint("CK_Transaction_Price_Range",
                $"Price >= {ValidationConstants.Transaction.PriceMin} AND Price <= {ValidationConstants.Transaction.PriceMax}");
        });

        builder.HasOne(t => t.Post)
            .WithMany(p=>p.Transactions)
            .HasForeignKey(t => t.PostId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.TransactionStatus)
            .WithMany(t => t.Transactions)
            .HasForeignKey(t => t.TransactionStatusId);

        builder.Property(t => t.InitiatorId)
            .IsRequired();
        
        builder.HasOne(t => t.Initiator)
            .WithMany()
            .HasForeignKey(t => t.InitiatorId);
        
        builder.HasOne(t => t.Buyer)
            .WithMany(u => u.TransactionsAsBuyer)
            .HasForeignKey(t => t.BuyerId);
        
        builder.HasOne(t => t.Seller)
            .WithMany(u => u.TransactionsAsSeller)
            .HasForeignKey(t => t.SellerId);
        
        builder.Property(t => t.BuyerConfirmed)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(t => t.SellerConfirmed)
            .IsRequired()
            .HasDefaultValue(false);
        
        builder.Property(t => t.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
        
        builder.Property(p => p.UpdatedAt);
        
        builder.Property(t => t.TransactionDate)
            .HasColumnType("date");
        
        builder.Property(t => t.CompletedAt)
            .IsRequired(false)
            .HasDefaultValue(null);
    }
}