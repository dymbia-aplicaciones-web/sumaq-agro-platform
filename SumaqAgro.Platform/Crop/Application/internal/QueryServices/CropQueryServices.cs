using SumaqAgro.Platform.Crop.Domain.Model.Querys;
using SumaqAgro.Platform.Crop.Domain.Model.Aggregates;
using SumaqAgro.Platform.Crop.Application.QueryServices;
using SumaqAgro.Platform.Crop.Domain.Repositories;

namespace SumaqAgro.Platform.Crop.Application.Internal.QueryServices;

public sealed class CropQueryService(ICropRepository repository) : ICropQueryService
{
    public Task<PestReport?> FindReportAsync(FindReportQuery request, CancellationToken ct) =>
        repository.FindReportAsync(request.Id, ct);

    public Task<FieldInspection?> FindInspectionAsync(FindInspectionQuery request, CancellationToken ct) =>
        repository.FindInspectionAsync(request.Id, ct);

    public Task<TechnicalPrescription?> FindPrescriptionAsync(FindPrescriptionQuery request, CancellationToken ct) =>
        repository.FindPrescriptionAsync(request.Id, ct);

    public Task<List<PestReport>> ListReportsAsync(ListReportsQuery request, CancellationToken ct) =>
        repository.ListReportsAsync(request.PlotId, ct);

    public Task<List<FieldInspection>> ListInspectionsAsync(ListInspectionsQuery request, CancellationToken ct) =>
        repository.ListInspectionsAsync(request.PlotId, ct);

    public Task<List<TechnicalPrescription>> ListPrescriptionsAsync(ListPrescriptionsQuery request, CancellationToken ct) =>
        repository.ListPrescriptionsAsync(request.PlotId, ct);

    public Task<List<SatelliteObservation>> ListObservationsAsync(ListObservationsQuery request, CancellationToken ct) =>
        repository.ListObservationsAsync(request.PlotId, ct);

    public Task<List<ClimateForecast>> ListForecastsAsync(ListForecastsQuery request, CancellationToken ct) =>
        repository.ListForecastsAsync(request.Region, ct);

    public Task<List<AgroclimaticAlert>> ListAlertsAsync(ListAlertsQuery request, CancellationToken ct) =>
        repository.ListAlertsAsync(request.Region, ct);

    public Task<List<RegionalBulletin>> ListBulletinsAsync(ListBulletinsQuery request, CancellationToken ct) =>
        repository.ListBulletinsAsync(request.Region, ct);
}
