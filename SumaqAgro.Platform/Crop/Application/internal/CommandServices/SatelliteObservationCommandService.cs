using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using SumaqAgro.Platform.Crop.Application.CommandServices;
using SumaqAgro.Platform.Crop.Application.Internal.OutboundServices;
using SumaqAgro.Platform.Crop.Domain.Model.Aggregates;
using SumaqAgro.Platform.Crop.Domain.Model.Commands;
using SumaqAgro.Platform.Crop.Domain.Repositories;
using SumaqAgro.Platform.Shared.Application.Model;
using SumaqAgro.Platform.Shared.Domain.Model.Errors;
using SumaqAgro.Platform.Shared.Domain.Repositories;

namespace SumaqAgro.Platform.Crop.Application.Internal.CommandServices;

public sealed class SatelliteObservationCommandService(IMonitoringPlots plots, ISatelliteImagery imagery,
    ICropRepository repository, IUnitOfWork work) : ISatelliteObservationCommandService
{
    public async Task<Result<SatelliteObservation>> ImportAsync(ImportSatelliteObservationCommand command, CancellationToken ct)
    {
        var start = command.Start.ToUnixTimeSeconds();
        var end = command.End.ToUnixTimeSeconds();
        if (start >= end) return Fail(Error.Validation("SatelliteRange", "Start must precede end at whole-second precision."));

        var plot = await plots.FindOwnedAsync(command.PlotId, command.UserId, ct);
        if (plot is null)
            return Fail(Error.NotFound("FieldPlot", "Field plot not found."));
        if (string.IsNullOrWhiteSpace(plot.PolygonId))
            return Fail(Error.Conflict("FieldPlot", "The plot must be linked to a monitoring polygon."));

        SatelliteMeasurement? capture;
        try { capture = await imagery.FindLatestAsync(plot.PolygonId, start, end, ct); }
        catch (Exception ex) when (ex is HttpRequestException or JsonException ||
                                    ex is OperationCanceledException && !ct.IsCancellationRequested)
        {
            return Fail(Error.Unexpected("SatelliteProvider", "Could not read measurements from AgroMonitoring."));
        }
        if (capture is null) return Fail(Error.NotFound("SatelliteObservation", "No observation found in this period."));

        var identity = FormattableString.Invariant($"{command.PlotId}|{capture.Timestamp}|{capture.Source}");
        var key = Convert.ToHexStringLower(SHA256.HashData(Encoding.UTF8.GetBytes(identity)));
        var existing = await repository.FindImportedObservationAsync(key, ct);
        if (existing is not null) return Result<SatelliteObservation>.Success(existing);

        var observation = new SatelliteObservation(command.PlotId, DateTimeOffset.FromUnixTimeSeconds(capture.Timestamp),
            capture.Source, capture.Ndvi, capture.Ndwi, capture.Clouds, null);
        observation.MarkImported(key);
        repository.Add(observation);
        try { await work.CompleteAsync(ct); }
        catch (DbUpdateException ex) when (ex.InnerException is MySqlException { Number: 1062 })
        {
            existing = await repository.FindImportedObservationAsync(key, ct);
            if (existing is null) throw;
            return Result<SatelliteObservation>.Success(existing);
        }

        return Result<SatelliteObservation>.Success(observation);
    }

    private static Result<SatelliteObservation> Fail(Error error) => Result<SatelliteObservation>.Failure(error);
}
