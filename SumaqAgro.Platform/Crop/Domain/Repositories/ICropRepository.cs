using SumaqAgro.Platform.Crop.Domain.Model.Aggregates;

namespace SumaqAgro.Platform.Crop.Domain.Repositories;

public interface ICropRepository
{
    Task<PestReport?> FindReportAsync(long id, CancellationToken ct);
    Task<FieldInspection?> FindInspectionAsync(long id, CancellationToken ct);
    Task<TechnicalPrescription?> FindPrescriptionAsync(long id, CancellationToken ct);
    Task<List<PestReport>> ListReportsAsync(long plotId, CancellationToken ct);
    Task<List<FieldInspection>> ListInspectionsAsync(long plotId, CancellationToken ct);
    Task<List<TechnicalPrescription>> ListPrescriptionsAsync(long plotId, CancellationToken ct);
    Task<List<SatelliteObservation>> ListObservationsAsync(long plotId, CancellationToken ct);
    Task<List<ClimateForecast>> ListForecastsAsync(string region, CancellationToken ct);
    Task<List<AgroclimaticAlert>> ListAlertsAsync(string region, CancellationToken ct);
    Task<List<RegionalBulletin>> ListBulletinsAsync(string region, CancellationToken ct);
    void Add<T>(T entity) where T : class;
    void Remove(PestReport report);
}
