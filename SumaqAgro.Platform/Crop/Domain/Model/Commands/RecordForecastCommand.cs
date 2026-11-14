using SumaqAgro.Platform.Crop.Domain.Model.Aggregates;

namespace SumaqAgro.Platform.Crop.Domain.Model.Commands;

public sealed record RecordForecastCommand(ClimateForecast ClimateForecast);
