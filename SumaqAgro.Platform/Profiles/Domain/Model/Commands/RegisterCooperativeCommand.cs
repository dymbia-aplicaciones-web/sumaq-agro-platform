using SumaqAgro.Platform.Profiles.Domain.Model.ValueObjects;

namespace SumaqAgro.Platform.Profiles.Domain.Model.Commands;

public sealed record RegisterCooperativeCommand(Ruc Ruc, string Name, string Region, long ManagerId);
