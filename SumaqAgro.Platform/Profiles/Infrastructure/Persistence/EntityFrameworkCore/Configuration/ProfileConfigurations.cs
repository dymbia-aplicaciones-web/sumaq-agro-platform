using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SumaqAgro.Platform.Iam.Domain.Model.Aggregates;
using SumaqAgro.Platform.Profiles.Domain.Model.Aggregates;
using SumaqAgro.Platform.Profiles.Domain.Model.Entities;

namespace SumaqAgro.Platform.Profiles.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

public sealed class ProfileConfiguration : IEntityTypeConfiguration<Profile>
{
    public void Configure(EntityTypeBuilder<Profile> builder)
    {
        builder.HasIndex(profile => profile.UserId).IsUnique();
        builder.HasOne(profile => profile.User).WithOne().HasForeignKey<Profile>(profile => profile.UserId);
        builder.Property(profile => profile.FirstName).HasMaxLength(100);
        builder.Property(profile => profile.LastName).HasMaxLength(100);
        builder.Property(profile => profile.PhoneNumber).HasMaxLength(15);
        builder.Property(profile => profile.PreferredLanguage).HasMaxLength(2);
    }
}

public sealed class CooperativeConfiguration : IEntityTypeConfiguration<Cooperative>
{
    public void Configure(EntityTypeBuilder<Cooperative> builder)
    {
        builder.HasIndex(cooperative => cooperative.Ruc).IsUnique();
        builder.Property(cooperative => cooperative.Ruc).HasMaxLength(11);
        builder.Property(cooperative => cooperative.BusinessName).HasMaxLength(255);
        builder.Property(cooperative => cooperative.Region).HasMaxLength(255);
    }
}

public sealed class CooperativeMemberConfiguration : IEntityTypeConfiguration<CooperativeMember>
{
    public void Configure(EntityTypeBuilder<CooperativeMember> builder)
    {
        builder.HasOne<Cooperative>().WithMany().HasForeignKey(member => member.CooperativeId);
        builder.HasIndex(member => new { member.CooperativeId, member.Dni }).IsUnique();
        builder.Property(member => member.Dni).HasMaxLength(8);
        builder.Property(member => member.FirstName).HasMaxLength(100);
        builder.Property(member => member.LastName).HasMaxLength(100);
        builder.Property(member => member.Community).HasMaxLength(100);
    }
}

public sealed class AgronomistInvitationConfiguration : IEntityTypeConfiguration<AgronomistInvitation>
{
    public void Configure(EntityTypeBuilder<AgronomistInvitation> builder)
    {
        builder.HasOne<Cooperative>().WithMany().HasForeignKey(invitation => invitation.CooperativeId);
        builder.Property(invitation => invitation.Email).HasMaxLength(255);
        builder.Property(invitation => invitation.Status).HasMaxLength(16);
    }
}

public sealed class AgronomistAssignmentConfiguration : IEntityTypeConfiguration<AgronomistAssignment>
{
    public void Configure(EntityTypeBuilder<AgronomistAssignment> builder)
    {
        builder.Property<long?>("ActivePlotId").HasComputedColumnSql(
            "CASE WHEN active THEN plot_id ELSE NULL END", stored: true);
        builder.HasIndex("ActivePlotId").IsUnique();
        builder.HasIndex(assignment => assignment.CooperativeId);
    }
}
