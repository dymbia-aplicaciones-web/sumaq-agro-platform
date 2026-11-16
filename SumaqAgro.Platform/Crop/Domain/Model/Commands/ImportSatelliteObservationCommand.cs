namespace SumaqAgro.Platform.Crop.Domain.Model.Commands;

public sealed record ImportSatelliteObservationCommand(long PlotId, long UserId, DateTimeOffset Start, DateTimeOffset End);
