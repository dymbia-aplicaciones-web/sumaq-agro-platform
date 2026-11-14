using SumaqAgro.Platform.Crop.Domain.Model.Querys;
using SumaqAgro.Platform.Crop.Domain.Model.Aggregates;

namespace SumaqAgro.Platform.Crop.Application.QueryServices;

public interface ICropQueryService
{
    Task<PestReport?> FindReportAsync(FindReportQuery request, CancellationToken ct);

    Task<FieldInspection?> FindInspectionAsync(FindInspectionQuery request, CancellationToken ct);

    Task<TechnicalPrescription?> FindPrescriptionAsync(FindPrescriptionQuery request, CancellationToken ct);

    Task<List<PestReport>> ListReportsAsync(ListReportsQuery request, CancellationToken ct);

    Task<List<FieldInspection>> ListInspectionsAsync(ListInspectionsQuery request, CancellationToken ct);

    Task<List<TechnicalPrescription>> ListPrescriptionsAsync(ListPrescriptionsQuery request, CancellationToken ct);

    Task<List<SatelliteObservation>> ListObservationsAsync(ListObservationsQuery request, CancellationToken ct);

    Task<List<ClimateForecast>> ListForecastsAsync(ListForecastsQuery request, CancellationToken ct);

    Task<List<AgroclimaticAlert>> ListAlertsAsync(ListAlertsQuery request, CancellationToken ct);

    Task<List<RegionalBulletin>> ListBulletinsAsync(ListBulletinsQuery request, CancellationToken ct);
}
