using SumaqAgro.Platform.Certification.Domain.Model.Entities;
using SumaqAgro.Platform.Certification.Domain.Model.ValueObjects;
namespace SumaqAgro.Platform.Certification.Domain.Model.Aggregates;

public sealed class HarvestBatch(string batchCode, long cooperativeId, long memberId, long plotId, string cropType,
    DateOnly harvestedOn, DateTimeOffset registeredAt)
{
    public long Id { get; private set; }
    public string BatchCode { get; private set; } = batchCode;
    public long CooperativeId { get; private set; } = cooperativeId;
    public long MemberId { get; private set; } = memberId;
    public long PlotId { get; private set; } = plotId;
    public string CropType { get; private set; } = cropType;
    public DateOnly HarvestedOn { get; private set; } = harvestedOn;
    public DateTimeOffset RegisteredAt { get; private set; } = registeredAt;
    public string Status { get; private set; } = "PENDING";
    public decimal? NetWeightKg { get; private set; }
    public string? SampleIdentifier { get; private set; }
    public decimal? SampleWeightKg { get; private set; }
    public HarvestSample? RepresentativeSample =>
        SampleIdentifier is { } identifier && SampleWeightKg is { } weight
            ? new HarvestSample(identifier, weight)
            : null;
    public decimal? FirstCaliberPercentage { get; private set; }
    public decimal? SecondCaliberPercentage { get; private set; }
    public decimal? ThirdCaliberPercentage { get; private set; }
    public decimal? PestDamagePercentage { get; private set; }
    public decimal? CoffeeCuppingScore { get; private set; }
    public bool CoffeeFermentDefect { get; private set; }
    public decimal? QualityScore { get; private set; }
    public string? QualityCategory { get; private set; }

    public void Weigh(decimal gross, decimal tare)
    {
        Require(Status == "PENDING" && NetWeightKg is null, "Only a pending, unweighed batch can be weighed.");
        Require(tare >= 0 && gross > tare, "Net weight must be positive and tare cannot be negative.");
        NetWeightKg = gross - tare;
    }

    public void Sample(string identifier, decimal weight)
    {
        Require(Status == "PENDING" && NetWeightKg is not null && SampleIdentifier is null,
            "The batch must be pending, weighed and without a sample.");
        Require(weight > 0 && weight <= NetWeightKg && (CropType != "SPECIALTY_COFFEE" || weight >= 1),
            "Sample weight is outside the allowed range.");
        var sample = new HarvestSample(identifier, weight);
        SampleIdentifier = sample.Identifier;
        SampleWeightKg = sample.WeightKg;
    }

    public void GradePotato(PotatoCaliberDistribution distribution)
    {
        Require(Status == "PENDING" && SampleIdentifier is not null && CropType == "ANDEAN_POTATO",
            "Potato grading requires a pending, sampled potato batch.");
        Require(distribution.HasValidTotal, "Potato caliber shares must add up to 100.");
        FirstCaliberPercentage = distribution.First;
        SecondCaliberPercentage = distribution.Second;
        ThirdCaliberPercentage = distribution.Third;
        PestDamagePercentage = distribution.Damage;
        QualityScore = distribution.First;
        QualityCategory = distribution.Damage > 15 ? "B_GRADE_REQUIRES_SORTING" : distribution.First >= 50 ? "PREMIUM_GOLD" : "STANDARD";
        Status = "GRADED";
    }

    public void CupCoffee(decimal score, bool fermentDefect)
    {
        Require(Status == "PENDING" && SampleIdentifier is not null && CropType == "SPECIALTY_COFFEE",
            "Cupping requires a pending, sampled coffee batch.");
        CoffeeCuppingScore = score;
        CoffeeFermentDefect = fermentDefect;
        QualityScore = score;
        QualityCategory = score >= 80 && !fermentDefect ? "SPECIALTY_COFFEE" : "COMMERCIAL_COFFEE";
        Status = "GRADED";
    }

    public void Certify()
    {
        Require(Status == "GRADED" && QualityCategory is not null, "The batch needs an assigned quality score.");
        Status = "CERTIFIED";
    }

    private static void Require(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
    }
}
