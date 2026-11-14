using SumaqAgro.Platform.Crop.Domain.Model.ValueObjects;
using SumaqAgro.Platform.Crop.Domain.Model.Commands;
using SumaqAgro.Platform.Crop.Application.CommandServices;
using SumaqAgro.Platform.Crop.Domain.Model.Aggregates;
using SumaqAgro.Platform.Crop.Domain.Repositories;
using SumaqAgro.Platform.Shared.Domain.Repositories;

namespace SumaqAgro.Platform.Crop.Application.Internal.CommandServices;

public sealed class CropMonitoringService(ICropRepository repository, IUnitOfWork unitOfWork) : ICropMonitoringService
{
    public Task RecordObservationAsync(RecordObservationCommand command, CancellationToken ct)
    {
        var observation = command.SatelliteObservation;
        if (observation.PlotId <= 0 || string.IsNullOrWhiteSpace(observation.Source) ||
            (observation.NdviMean is null && observation.NdwiMean is null) ||
            (observation.NdviMean is double ndvi && !Within(ndvi, -1, 1)) ||
            (observation.NdwiMean is double ndwi && !Within(ndwi, -1, 1)) ||
            !Within(observation.CloudCoveragePercent, 0, 100) ||
            (observation.SurfaceTempKelvin is double temp && !Within(temp, 0, double.MaxValue)))
            throw new ArgumentException("Invalid satellite observation.", nameof(observation));

        return SaveAsync(observation, ct);
    }

    public Task RecordForecastAsync(RecordForecastCommand command, CancellationToken ct)
    {
        var forecast = command.ClimateForecast;
        if (string.IsNullOrWhiteSpace(forecast.Region) || string.IsNullOrWhiteSpace(forecast.Source) ||
            !Within(forecast.TemperatureCelsius, -273.15, double.MaxValue) || !Within(forecast.RainChance, 0, 100))
            throw new ArgumentException("Invalid climate forecast.", nameof(forecast));

        return SaveAsync(forecast, ct);
    }

    public Task IssueAlertAsync(IssueAlertCommand command, CancellationToken ct)
    {
        var alert = command.AgroclimaticAlert;
        if (string.IsNullOrWhiteSpace(alert.Title) || string.IsNullOrWhiteSpace(alert.Description) ||
            string.IsNullOrWhiteSpace(alert.Region) || string.IsNullOrWhiteSpace(alert.Source) ||
            !Enum.GetNames<AgroclimaticAlertSeverity>().Contains(alert.Severity, StringComparer.Ordinal))
            throw new ArgumentException("Invalid agroclimatic alert.", nameof(alert));

        return SaveAsync(alert, ct);
    }

    private Task SaveAsync<T>(T entry, CancellationToken ct) where T : class
    {
        repository.Add(entry);
        return unitOfWork.CompleteAsync(ct);
    }

    private static bool Within(double value, double minimum, double maximum) =>
        double.IsFinite(value) && value >= minimum && value <= maximum;
}
