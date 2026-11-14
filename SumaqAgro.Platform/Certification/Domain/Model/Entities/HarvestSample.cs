namespace SumaqAgro.Platform.Certification.Domain.Model.Entities;

public sealed class HarvestSample
{
    public HarvestSample(string identifier, decimal weightKg)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(identifier);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(weightKg);
        Identifier = identifier;
        WeightKg = weightKg;
    }

    public string Identifier { get; }
    public decimal WeightKg { get; }
}