namespace SumaqAgro.Platform.Profiles.Domain.Model.Commands;

public sealed record AssignAgronomistCommand(long CooperativeId, long AgronomistUserId, long PlotId);
