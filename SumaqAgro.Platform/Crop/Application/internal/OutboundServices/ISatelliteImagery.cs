namespace SumaqAgro.Platform.Crop.Application.Internal.OutboundServices;

public interface ISatelliteImagery
{
    Task<SatelliteMeasurement?> FindLatestAsync(string polygonId, long start, long end, CancellationToken ct);
}

public sealed record SatelliteMeasurement(long Timestamp, string Source, double? Ndvi, double? Ndwi, double Clouds);
