using SumaqAgro.Platform.Farming.Domain.Model.ValueObjects;

namespace SumaqAgro.Platform.Farming.Domain.Model.Aggregates;

public sealed class FieldPlot(long ownerUserId, string name, string region, decimal areaHectares)
{
    public long Id { get; private set; }
    public long OwnerUserId { get; private set; } = ownerUserId;
    public string Name { get; private set; } = name;
    public string Region { get; private set; } = region;
    public string Status { get; private set; } = "WITHOUT_POLYGON";
    public decimal AreaHectares { get; private set; } = areaHectares;
    public string? AgroMonitoringPolygonId { get; private set; }
    public GeoCoordinate[]? Boundary { get; private set; }

    public void Delineate(PlotBoundary boundary)
    {
        Boundary = boundary.Vertices;
        AreaHectares = boundary.Area;
        AgroMonitoringPolygonId = null;
        Status = "WITHOUT_POLYGON";
    }

    public void LinkPolygon(string polygonId)
    {
        AgroMonitoringPolygonId = polygonId;
        Status = "ACTIVE_MONITORING";
    }
}
