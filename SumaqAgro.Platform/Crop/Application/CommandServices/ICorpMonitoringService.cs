using SumaqAgro.Platform.Crop.Domain.Model.Commands;
using SumaqAgro.Platform.Crop.Domain.Model.Aggregates;

namespace SumaqAgro.Platform.Crop.Application.CommandServices;

public interface ICropMonitoringService
{
    Task RecordObservationAsync(RecordObservationCommand command, CancellationToken ct);

    Task RecordForecastAsync(RecordForecastCommand command, CancellationToken ct);

    Task IssueAlertAsync(IssueAlertCommand command, CancellationToken ct);
}
