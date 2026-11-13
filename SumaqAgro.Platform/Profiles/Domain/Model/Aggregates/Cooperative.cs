namespace SumaqAgro.Platform.Profiles.Domain.Model.Aggregates;

public sealed class Cooperative(string ruc, string businessName, string region, long managerUserId)
{
    public long Id { get; private set; }
    public string Ruc { get; private set; } = ruc;
    public string BusinessName { get; private set; } = businessName;
    public string Region { get; private set; } = region;
    public long ManagerUserId { get; private set; } = managerUserId;
}
