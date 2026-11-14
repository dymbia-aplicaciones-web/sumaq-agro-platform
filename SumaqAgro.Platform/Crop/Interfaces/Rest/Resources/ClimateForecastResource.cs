namespace SumaqAgro.Platform.Crop.Interfaces.Rest.Resources;

public sealed record ClimateForecastResource(long Id, string Region, double TemperatureCelsius, double RainChance, DateTimeOffset ForecastDate, string Source);
