namespace SumaqAgro.Platform.Profiles.Domain.Model.Aggregates;

public sealed class AgronomistAssignment(long cooperativeId, long agronomistUserId, long plotId)
{
    public List<AssignmentHistoryEntry> History { get; private set; } = [];

    public void Reassign(long userId, DateTime now)
    {
        if (AgronomistUserId == userId) return;
        History.Add(new AssignmentHistoryEntry(AgronomistUserId, AssignedAt, now));
        AgronomistUserId = userId;
        AssignedAt = now;
    }

    public long Id { get; private set; }
    public long CooperativeId { get; private set; } = cooperativeId;
    public long AgronomistUserId { get; private set; } = agronomistUserId;
    public long PlotId { get; private set; } = plotId;
    public DateTime AssignedAt { get; private set; } = DateTime.UtcNow;
    public bool Active { get; private set; } = true;
}

public sealed record AssignmentHistoryEntry(long AgronomistUserId, DateTime AssignedAt, DateTime EndedAt);
