using SumaqAgro.Platform.Crop.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using SumaqAgro.Platform.Crop.Domain.Model.Aggregates;
using SumaqAgro.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

namespace SumaqAgro.Platform.Crop.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public sealed class CropRepository(AppDbContext context) : ICropRepository
{
    public Task<PestReport?> FindReportAsync(long id, CancellationToken ct) =>
        context.Set<PestReport>().SingleOrDefaultAsync(report => report.Id == id, ct);

    public Task<FieldInspection?> FindInspectionAsync(long id, CancellationToken ct) =>
        context.Set<FieldInspection>().SingleOrDefaultAsync(inspection => inspection.Id == id, ct);

    public Task<TechnicalPrescription?> FindPrescriptionAsync(long id, CancellationToken ct) =>
        context.Set<TechnicalPrescription>().SingleOrDefaultAsync(prescription => prescription.Id == id, ct);

    public Task<List<PestReport>> ListReportsAsync(long plotId, CancellationToken ct) =>
        context.Set<PestReport>().AsNoTracking().Where(report => report.PlotId == plotId).OrderBy(report => report.Id).ToListAsync(ct);

    public Task<List<FieldInspection>> ListInspectionsAsync(long plotId, CancellationToken ct) =>
        context.Set<FieldInspection>().AsNoTracking().Where(inspection => inspection.PlotId == plotId)
            .OrderBy(inspection => inspection.Id).ToListAsync(ct);

    public Task<List<TechnicalPrescription>> ListPrescriptionsAsync(long plotId, CancellationToken ct) =>
        context.Set<TechnicalPrescription>().AsNoTracking().Where(prescription => prescription.PlotId == plotId)
            .OrderBy(prescription => prescription.Id).ToListAsync(ct);

    public Task<List<SatelliteObservation>> ListObservationsAsync(long plotId, CancellationToken ct) =>
        context.Set<SatelliteObservation>().AsNoTracking().Where(observation => observation.PlotId == plotId)
            .OrderByDescending(observation => observation.CapturedAt).ThenByDescending(observation => observation.Id).ToListAsync(ct);

    public Task<List<ClimateForecast>> ListForecastsAsync(string region, CancellationToken ct) =>
        context.Set<ClimateForecast>().AsNoTracking().Where(forecast => forecast.Region == region)
            .OrderByDescending(forecast => forecast.ForecastDate).ThenByDescending(forecast => forecast.Id).ToListAsync(ct);

    public Task<List<AgroclimaticAlert>> ListAlertsAsync(string region, CancellationToken ct) =>
        context.Set<AgroclimaticAlert>().AsNoTracking().Where(alert => alert.Region == region)
            .OrderByDescending(alert => alert.IssuedAt).ThenByDescending(alert => alert.Id).ToListAsync(ct);

    public Task<List<RegionalBulletin>> ListBulletinsAsync(string region, CancellationToken ct) =>
        context.Set<RegionalBulletin>().AsNoTracking().Where(bulletin => bulletin.Region == region)
            .OrderByDescending(bulletin => bulletin.PublishedAt).ThenByDescending(bulletin => bulletin.Id).ToListAsync(ct);

    public void Add<T>(T entity) where T : class => context.Add(entity);
    public void Remove(PestReport report) => context.Remove(report);
}
