using Agora.Core.Models;
using Agora.Core.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agora.Infrastructure.Configuration;

public class UserConfiguration: IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(u => u.UserName)
            .IsRequired()
            .HasMaxLength(ValidationRules.AppUser.UsernameMaxLength);
        builder.HasIndex(u => u.UserName).IsUnique();
        
        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(ValidationRules.AppUser.EmailMaxLength);
        builder.HasIndex(u => u.Email).IsUnique();
        
        builder.Property(u => u.PasswordHash)
            .IsRequired()
            .HasMaxLength(ValidationRules.AppUser.EmailMaxLength);
        builder.HasIndex(u => u.Email).IsUnique();
    }
}