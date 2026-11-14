using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SumaqAgro.Platform.Certification.Domain.Model.Aggregates;

namespace SumaqAgro.Platform.Certification.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

public sealed class HarvestBatchConfiguration : IEntityTypeConfiguration<HarvestBatch>
{
    public void Configure(EntityTypeBuilder<HarvestBatch> builder)
    {
        builder.HasIndex(batch => batch.BatchCode).IsUnique();
        builder.Ignore(batch => batch.RepresentativeSample);
        builder.Property(batch => batch.BatchCode).HasMaxLength(40);
        builder.Property(batch => batch.Status).IsConcurrencyToken();
        builder.Property(batch => batch.NetWeightKg).HasPrecision(12, 3).IsConcurrencyToken();
        builder.Property(batch => batch.SampleIdentifier).HasMaxLength(40).IsConcurrencyToken();
        builder.Property(batch => batch.SampleWeightKg).HasPrecision(12, 3);
    }
}

public sealed class QualityCertificateConfiguration : IEntityTypeConfiguration<QualityCertificate>
{
    public void Configure(EntityTypeBuilder<QualityCertificate> builder)
    {
        builder.HasOne<HarvestBatch>().WithOne().HasForeignKey<QualityCertificate>(certificate => certificate.HarvestBatchId);
        builder.HasIndex(certificate => certificate.CertificateNumber).IsUnique();
        builder.Property(certificate => certificate.CertificateNumber).HasMaxLength(40);
        builder.Property(certificate => certificate.CertificateHash).HasMaxLength(64);
        builder.Property(certificate => certificate.Status).IsConcurrencyToken();
    }
}