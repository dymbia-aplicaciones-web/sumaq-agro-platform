namespace SumaqAgro.Platform.Crop.Domain.Model.Aggregates;

public sealed class ClimateForecast(string region, double temperatureCelsius, double rainChance, DateTimeOffset forecastDate, string source)
{
    public long Id { get; private set; }
    public string Region { get; private set; } = region;
    public double TemperatureCelsius { get; private set; } = temperatureCelsius;
    public double RainChance { get; private set; } = rainChance;
    public DateTimeOffset ForecastDate { get; private set; } = forecastDate;
    public string Source { get; private set; } = source;
}
