namespace SumaqAgro.Platform.Crop.Interfaces.Rest.Resources;

public sealed record SatelliteObservationResource(long Id, long PlotId, DateTimeOffset CapturedAt, string Source, double? NdviMean, double? NdwiMean, double CloudCoveragePercent, double? SurfaceTempKelvin);
