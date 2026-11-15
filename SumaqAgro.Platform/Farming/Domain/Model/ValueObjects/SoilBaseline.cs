namespace SumaqAgro.Platform.Farming.Domain.Model.ValueObjects;

public sealed record SoilBaseline(decimal Ph, string Texture, decimal OrganicMatterPercentage, DateOnly RecordedAt);
