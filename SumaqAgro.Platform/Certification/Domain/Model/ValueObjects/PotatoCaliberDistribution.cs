namespace SumaqAgro.Platform.Certification.Domain.Model.ValueObjects;

public sealed record PotatoCaliberDistribution(decimal First, decimal Second, decimal Third, decimal Damage)
{
    public bool HasValidTotal => First + Second + Third == 100;
}