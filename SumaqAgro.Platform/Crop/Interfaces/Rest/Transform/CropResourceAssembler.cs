using SumaqAgro.Platform.Crop.Domain.Model.ValueObjects;
using SumaqAgro.Platform.Crop.Domain.Model.Aggregates;
using SumaqAgro.Platform.Crop.Domain.Model.Commands;
using SumaqAgro.Platform.Crop.Interfaces.Rest.Resources;
using SumaqAgro.Platform.Shared.Application.Model;

namespace SumaqAgro.Platform.Crop.Interfaces.Rest.Transform;

internal static class CropResourceAssembler
{
    public static ScheduleCommand ToCommand(ScheduleFieldInspectionResource resource) =>
        new(resource.PestReportId, resource.AgronomistId, resource.ScheduledAt!.Value);

    public static CompleteCommand ToCommand(long fieldInspectionId, CompleteFieldInspectionResource resource) =>
        new(fieldInspectionId, resource.Findings.Trim());

    public static ReportCommand ToCommand(RecordPestReportResource resource) =>
        new(resource.PlotId, resource.ReporterId, resource.Description.Trim(), Enum.Parse<PestReportSeverity>(resource.Severity));

    public static PublishCommand ToCommand(PublishRegionalBulletinResource resource) =>
        new(resource.AgronomistId, resource.Region.Trim(), resource.Title.Trim(),
            resource.Content.Trim());

    public static IssueCommand ToCommand(IssueTechnicalPrescriptionResource resource) =>
        new(resource.PestReportId, resource.AgronomistId, resource.Product.Trim(),
            resource.Dosage.Trim(), resource.Instructions.Trim(), resource.ApplicationDate!.Value);

    public static AgroclimaticAlertResource ToResource(AgroclimaticAlert model) =>
        new(model.Id, model.Title, model.Description, model.Severity, model.Region, model.IssuedAt, model.Source);

    public static ClimateForecastResource ToResource(ClimateForecast model) =>
        new(model.Id, model.Region, model.TemperatureCelsius, model.RainChance, model.ForecastDate, model.Source);

    public static FieldInspectionResource ToResource(FieldInspection model) =>
        new(model.Id, model.PestReportId, model.PlotId, model.AgronomistId, model.ScheduledAt, model.Status, model.Findings, model.CompletedAt);

    public static PestReportResource ToResource(PestReport model) =>
        new(model.Id, model.PlotId, model.ReporterId, model.Description, model.Severity, model.Status);

    public static RegionalBulletinResource ToResource(RegionalBulletin model) =>
        new(model.Id, model.AgronomistId, model.Region, model.Title, model.Content, model.PublishedAt);

    public static SatelliteObservationResource ToResource(SatelliteObservation model) =>
        new(model.Id, model.PlotId, model.CapturedAt, model.Source, model.NdviMean, model.NdwiMean, model.CloudCoveragePercent, model.SurfaceTempKelvin);

    public static TechnicalPrescriptionResource ToResource(TechnicalPrescription model) =>
        new(model.Id, model.PestReportId, model.PlotId, model.AgronomistId, model.Product, model.Dosage, model.Instructions, model.ApplicationDate, model.Status, model.AppliedAt);

    public static Result<TResource> ToResource<TModel, TResource>(Result<TModel> result, Func<TModel, TResource> map) =>
        result.Match(value => Result<TResource>.Success(map(value)), Result<TResource>.Failure);
}
