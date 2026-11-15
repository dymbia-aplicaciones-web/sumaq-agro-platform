using SumaqAgro.Platform.Crop.Domain.Model.Aggregates;
using SumaqAgro.Platform.Crop.Domain.Model.Commands;
using SumaqAgro.Platform.Shared.Application.Model;

namespace SumaqAgro.Platform.Crop.Application.CommandServices;

public interface ISatelliteObservationCommandService
{
    Task<Result<SatelliteObservation>> ImportAsync(ImportSatelliteObservationCommand command, CancellationToken ct);
}
