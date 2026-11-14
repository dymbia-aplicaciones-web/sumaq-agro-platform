using SumaqAgro.Platform.Certification.Application.QueryServices;
using SumaqAgro.Platform.Certification.Domain.Model.Aggregates;
using SumaqAgro.Platform.Certification.Domain.Model.Commands;
using SumaqAgro.Platform.Certification.Interfaces.Rest.Resources;
using SumaqAgro.Platform.Shared.Application.Model;

namespace SumaqAgro.Platform.Certification.Interfaces.Rest.Transform;

internal static class CertificationResourceAssembler
{
    public static RegisterHarvestBatchCommand ToCommand(RegisterBatch resource) =>
        new(resource.BatchCode.Trim(), resource.CooperativeId, resource.MemberId, resource.PlotId, resource.CropType.Trim().ToUpperInvariant(), resource.HarvestedOn!.Value);

    public static WeighHarvestBatchCommand ToCommand(long id, WeighBatch resource) =>
        new(id, resource.GrossWeightKg!.Value, resource.TareKg!.Value);

    public static SampleHarvestBatchCommand ToCommand(long id, ExtractSample resource) =>
        new(id, resource.SampleIdentifier.Trim(), resource.SampleWeightKg!.Value);

    public static GradePotatoCalibersCommand ToCommand(long id, GradePotato resource) =>
        new(id, new(resource.FirstPercentage!.Value, resource.SecondPercentage!.Value, resource.ThirdPercentage!.Value, resource.PestDamagePercentage!.Value));

    public static CupCoffeeCommand ToCommand(long id, CupCoffee resource) =>
        new(id, resource.TotalScore!.Value, resource.FermentDefect);

    public static IssueQualityCertificateCommand ToCommand(IssueCertificate resource) =>
        new(resource.CertificateNumber.Trim(), resource.HarvestBatchId, resource.IssuedByUserId);

    public static HarvestBatchResource ToResource(HarvestBatch model) =>
        new(model.Id, model.BatchCode, model.CooperativeId, model.MemberId, model.PlotId, model.CropType, model.HarvestedOn, model.RegisteredAt, model.Status, model.NetWeightKg, model.SampleIdentifier, model.SampleWeightKg, model.FirstCaliberPercentage, model.SecondCaliberPercentage, model.ThirdCaliberPercentage, model.PestDamagePercentage, model.CoffeeCuppingScore, model.CoffeeFermentDefect, model.QualityScore, model.QualityCategory);

    public static QualityCertificateResource ToResource(QualityCertificate model) =>
        new(model.Id, model.CertificateNumber, model.HarvestBatchId, model.QualityCategory, model.CertificateHash, model.IssuedByUserId, model.IssuedAt, model.Status);

    public static TraceabilityResource ToResource(TraceabilityView model) =>
        new(model.CertificateNumber, model.Status, model.Authentic, model.BatchCode, model.CropType,
            model.QualityCategory, model.QualityScore, model.PlotName, model.Region, model.SeedVariety);

    public static Result<TResource> ToResource<TModel, TResource>(Result<TModel> result, Func<TModel, TResource> map) =>
        result.Match(value => Result<TResource>.Success(map(value)), Result<TResource>.Failure);
}
