namespace SumaqAgro.Platform.Certification.Application.Internal.OutboundServices;

public interface ITraceabilityPlots
{
    Task<TraceabilityPlot?> FindAsync(long plotId, CancellationToken ct);
}

public sealed record TraceabilityPlot(string Name, string Region);
