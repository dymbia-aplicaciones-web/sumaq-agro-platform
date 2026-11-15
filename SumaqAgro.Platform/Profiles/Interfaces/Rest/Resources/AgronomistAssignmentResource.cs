namespace SumaqAgro.Platform.Profiles.Interfaces.Rest.Resources;

public sealed record AgronomistAssignmentResource(long Id, long CooperativeId, long AgronomistUserId, long PlotId, DateTime AssignedAt, bool Active, IReadOnlyList<AssignmentHistoryEntryResource> History);

public sealed record AssignmentHistoryEntryResource(long AgronomistUserId, DateTime AssignedAt, DateTime EndedAt);
