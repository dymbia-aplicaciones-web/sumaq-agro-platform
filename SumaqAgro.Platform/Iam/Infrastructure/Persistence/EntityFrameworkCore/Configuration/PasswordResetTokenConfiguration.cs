using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SumaqAgro.Platform.Iam.Domain.Model.Aggregates;

namespace SumaqAgro.Platform.Iam.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

public sealed class PasswordResetTokenConfiguration : IEntityTypeConfiguration<PasswordResetToken>
{
    public void Configure(EntityTypeBuilder<PasswordResetToken> builder)
    {
        builder.ToTable("password_reset_tokens");
        builder.HasKey(token => token.Digest);
        builder.Property(token => token.Digest).HasMaxLength(64);
        builder.Property(token => token.Used).IsConcurrencyToken();
        builder.HasOne<User>().WithMany().HasForeignKey(token => token.UserId);
    }
}
