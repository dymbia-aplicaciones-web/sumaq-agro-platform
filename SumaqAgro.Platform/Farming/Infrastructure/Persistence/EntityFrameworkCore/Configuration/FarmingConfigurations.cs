using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SumaqAgro.Platform.Farming.Domain.Model.Aggregates;
using SumaqAgro.Platform.Farming.Domain.Model.ValueObjects;

namespace SumaqAgro.Platform.Farming.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

public sealed class FieldPlotConfiguration : IEntityTypeConfiguration<FieldPlot>
{
    public void Configure(EntityTypeBuilder<FieldPlot> builder)
    {
        builder.HasIndex(plot => new { plot.OwnerUserId, plot.Name }).IsUnique();
        builder.Property(plot => plot.Name).HasMaxLength(100);
        builder.Property(plot => plot.Region).HasMaxLength(150);
        builder.Property(plot => plot.AreaHectares).HasPrecision(10, 2);
        builder.Property(plot => plot.Boundary).HasConversion(
            ring => JsonSerializer.Serialize(ring, (JsonSerializerOptions?)null),
            json => JsonSerializer.Deserialize<GeoCoordinate[]>(json, (JsonSerializerOptions?)null),
            new ValueComparer<GeoCoordinate[]?>(
                (left, right) => left == right || (left != null && right != null && left.SequenceEqual(right)),
                ring => ring == null ? 0 : ring.Aggregate(0, (hash, point) => HashCode.Combine(hash, point)),
                ring => ring == null ? null : ring.ToArray()));
        builder.Property(plot => plot.AgroMonitoringPolygonId).IsConcurrencyToken();
    }
}

public sealed class CropCampaignConfiguration : IEntityTypeConfiguration<CropCampaign>
{
    public void Configure(EntityTypeBuilder<CropCampaign> builder)
    {
        builder.HasOne<FieldPlot>().WithMany().HasForeignKey(campaign => campaign.FieldPlotId);
        builder.Property(campaign => campaign.Status).IsConcurrencyToken().HasMaxLength(16);
        builder.Property<long?>("ActiveFieldPlotId").HasComputedColumnSql(
            "CASE WHEN status = 'IN_PROGRESS' THEN field_plot_id ELSE NULL END", stored: true);
        builder.HasIndex("ActiveFieldPlotId").IsUnique();
    }
}
