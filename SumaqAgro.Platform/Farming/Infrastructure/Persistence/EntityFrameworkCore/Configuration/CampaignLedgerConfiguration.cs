using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SumaqAgro.Platform.Farming.Domain.Model.Aggregates;

namespace SumaqAgro.Platform.Farming.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

public sealed class CampaignLedgerConfiguration : IEntityTypeConfiguration<CampaignLedger>
{
    public void Configure(EntityTypeBuilder<CampaignLedger> builder)
    {
        builder.ToTable("campaign_ledgers");
        builder.HasKey(ledger => ledger.CampaignId);
        builder.Property(ledger => ledger.CampaignId).ValueGeneratedNever();
        builder.Property(ledger => ledger.Payload).HasColumnType("json");
        builder.Property(ledger => ledger.Version).IsConcurrencyToken();
        builder.HasOne<CropCampaign>().WithOne().HasForeignKey<CampaignLedger>(ledger => ledger.CampaignId);
    }
}
