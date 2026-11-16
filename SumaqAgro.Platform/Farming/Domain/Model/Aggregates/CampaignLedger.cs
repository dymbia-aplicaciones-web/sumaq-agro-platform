namespace SumaqAgro.Platform.Farming.Domain.Model.Aggregates;

public sealed class CampaignLedger(long campaignId)
{
    public long CampaignId { get; private set; } = campaignId;
    public string Payload { get; private set; } = "{}";
    public Guid Version { get; private set; } = Guid.NewGuid();

    public void Save(string payload)
    {
        Payload = payload;
        Version = Guid.NewGuid();
    }
}
