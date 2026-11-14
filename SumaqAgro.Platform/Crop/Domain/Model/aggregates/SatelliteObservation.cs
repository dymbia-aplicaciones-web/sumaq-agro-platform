namespace SumaqAgro.Platform.Crop.Domain.Model.Aggregates;

public sealed class SatelliteObservation(long plotId, DateTimeOffset capturedAt, string source, double? ndviMean, double? ndwiMean, double cloudCoveragePercent, double? surfaceTempKelvin)
{
    public long Id { get; private set; }
    public long PlotId { get; private set; } = plotId;
    public DateTimeOffset CapturedAt { get; private set; } = capturedAt;
    public string Source { get; private set; } = source;
    public double? NdviMean { get; private set; } = ndviMean;
    public double? NdwiMean { get; private set; } = ndwiMean;
    public double CloudCoveragePercent { get; private set; } = cloudCoveragePercent;
    public double? SurfaceTempKelvin { get; private set; } = surfaceTempKelvin;
}
