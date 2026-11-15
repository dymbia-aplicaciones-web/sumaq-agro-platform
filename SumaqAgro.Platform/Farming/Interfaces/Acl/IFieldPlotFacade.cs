namespace SumaqAgro.Platform.Farming.Interfaces.Acl;

public interface IFieldPlotFacade
{
    Task<FieldPlotLocation?> FindLocationAsync(long plotId, CancellationToken ct);
    Task<FieldPlotMonitoring?> FindMonitoringAsync(long plotId, long ownerUserId, CancellationToken ct);
}

public sealed record FieldPlotLocation(string Name, string Region);
public sealed record FieldPlotMonitoring(string? PolygonId);
