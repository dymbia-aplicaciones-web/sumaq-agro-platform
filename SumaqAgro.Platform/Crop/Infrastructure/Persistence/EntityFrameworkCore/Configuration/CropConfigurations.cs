using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SumaqAgro.Platform.Crop.Domain.Model.Aggregates;

namespace SumaqAgro.Platform.Crop.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

public sealed class PestReportConfiguration : IEntityTypeConfiguration<PestReport>
{
    public void Configure(EntityTypeBuilder<PestReport> builder)
    {
        builder.HasIndex(report => report.PlotId);
        builder.Property(report => report.Status).HasMaxLength(16).IsConcurrencyToken();
    }
}

public sealed class FieldInspectionConfiguration : IEntityTypeConfiguration<FieldInspection>
{
    public void Configure(EntityTypeBuilder<FieldInspection> builder)
    {
        builder.HasIndex(inspection => inspection.PestReportId).IsUnique();
        builder.HasIndex(inspection => inspection.PlotId);
        builder.Property(inspection => inspection.Status).HasMaxLength(16).IsConcurrencyToken();
    }
}

public sealed class TechnicalPrescriptionConfiguration : IEntityTypeConfiguration<TechnicalPrescription>
{
    public void Configure(EntityTypeBuilder<TechnicalPrescription> builder)
    {
        builder.HasIndex(prescription => prescription.PestReportId).IsUnique();
        builder.HasIndex(prescription => prescription.PlotId);
        builder.Property(prescription => prescription.Status).HasMaxLength(16).IsConcurrencyToken();
    }
}

public sealed class RegionalBulletinConfiguration : IEntityTypeConfiguration<RegionalBulletin>
{
    public void Configure(EntityTypeBuilder<RegionalBulletin> builder)
    {
        builder.Property(bulletin => bulletin.Region).HasMaxLength(255);
        builder.HasIndex(bulletin => new { bulletin.Region, bulletin.PublishedAt });
    }
}

public sealed class SatelliteObservationConfiguration : IEntityTypeConfiguration<SatelliteObservation>
{
    public void Configure(EntityTypeBuilder<SatelliteObservation> builder)
    {
        builder.HasIndex(observation => new { observation.PlotId, observation.CapturedAt });
    }
}

public sealed class ClimateForecastConfiguration : IEntityTypeConfiguration<ClimateForecast>
{
    public void Configure(EntityTypeBuilder<ClimateForecast> builder)
    {
        builder.Property(forecast => forecast.Region).HasMaxLength(255);
        builder.HasIndex(forecast => new { forecast.Region, forecast.ForecastDate });
    }
}

public sealed class AgroclimaticAlertConfiguration : IEntityTypeConfiguration<AgroclimaticAlert>
{
    public void Configure(EntityTypeBuilder<AgroclimaticAlert> builder)
    {
        builder.Property(alert => alert.Region).HasMaxLength(255);
        builder.HasIndex(alert => new { alert.Region, alert.IssuedAt });
    }
}
