using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SumaqAgro.Platform.Iam.Domain.Model.Aggregates;

namespace SumaqAgro.Platform.Iam.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");
        builder.HasKey(user => user.Id);
        builder.HasIndex(user => user.Email).IsUnique();
        builder.Property(user => user.Email).HasMaxLength(254);
        builder.Property(user => user.PasswordHash).HasMaxLength(60);
        builder.Property(user => user.Role).HasMaxLength(32);
    }
}
