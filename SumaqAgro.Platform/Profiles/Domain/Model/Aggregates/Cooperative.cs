namespace SumaqAgro.Platform.Profiles.Domain.Model.Aggregates;

public sealed class Cooperative(string ruc, string businessName, string region, long managerUserId)
{
    public void Update(string updatedRuc, string name, string updatedRegion, long managerId)
    {
        Ruc = updatedRuc;
        BusinessName = name;
        Region = updatedRegion;
        ManagerUserId = managerId;
    }

    public long Id { get; private set; }
    public string Ruc { get; private set; } = ruc;
    public string BusinessName { get; private set; } = businessName;
    public string Region { get; private set; } = region;
    public long ManagerUserId { get; private set; } = managerUserId;
}
