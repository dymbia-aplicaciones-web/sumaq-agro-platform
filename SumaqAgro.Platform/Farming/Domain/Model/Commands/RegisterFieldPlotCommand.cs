namespace SumaqAgro.Platform.Farming.Domain.Model.Commands;

public sealed record RegisterFieldPlotCommand(long OwnerUserId, string Name, string Region, decimal AreaHectares);
