using SumaqAgro.Platform.Farming.Domain.Model.ValueObjects;

namespace SumaqAgro.Platform.Farming.Domain.Model.Commands;

public sealed record DelineateFieldPlotCommand(long Id, GeoCoordinate[] Vertices);
