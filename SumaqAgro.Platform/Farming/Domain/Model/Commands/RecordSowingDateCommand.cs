namespace SumaqAgro.Platform.Farming.Domain.Model.Commands;

public sealed record RecordSowingDateCommand(long Id, DateOnly Date);
