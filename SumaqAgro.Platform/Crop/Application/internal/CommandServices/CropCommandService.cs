using SumaqAgro.Platform.Crop.Domain.Model.Errors;
using SumaqAgro.Platform.Crop.Domain.Model.Commands;
using SumaqAgro.Platform.Crop.Application.CommandServices;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using SumaqAgro.Platform.Crop.Domain.Model.Aggregates;
using SumaqAgro.Platform.Crop.Domain.Repositories;
using SumaqAgro.Platform.Shared.Application.Model;
using SumaqAgro.Platform.Shared.Domain.Model.Errors;
using SumaqAgro.Platform.Shared.Domain.Repositories;

namespace SumaqAgro.Platform.Crop.Application.Internal.CommandServices;

public sealed class CropCommandService(ICropRepository repository, IUnitOfWork unitOfWork, TimeProvider time) : ICropCommandService
{
    private static readonly TimeZoneInfo BusinessZone = TimeZoneInfo.FindSystemTimeZoneById("America/Lima");

    public Task<Result<PestReport>> ReportAsync(ReportCommand request, CancellationToken ct) =>
        SaveNewAsync(new PestReport(request.PlotId, request.ReporterId, request.Description, request.Severity.ToString()), ct);

    public async Task<Result<string>> DeleteReportAsync(DeleteReportCommand request, CancellationToken ct)
    {
        var id = request.Id;
        var report = await repository.FindReportAsync(id, ct);
        if (report is null) return Result<string>.Failure(CropErrors.Missing("PestReport"));

        repository.Remove(report);
        try { await unitOfWork.CompleteAsync(ct); }
        catch (DbUpdateConcurrencyException)
        {
            return Result<string>.Failure(CropErrors.ReportChanged);
        }

        return Result<string>.Success($"Pest report with ID {id} successfully deleted.");
    }

    public async Task<Result<FieldInspection>> ScheduleAsync(ScheduleCommand request, CancellationToken ct)
    {
        var (reportId, agronomistId, scheduledAt) = request;
        var report = await repository.FindReportAsync(reportId, ct);
        if (report is null) return Result<FieldInspection>.Failure(CropErrors.Missing("PestReport"));
        if (report.Status != "PENDING")
            return Result<FieldInspection>.Failure(CropErrors.ReportMustBePending);
        if (scheduledAt <= time.GetUtcNow())
            return Result<FieldInspection>.Failure(CropErrors.InspectionInPast);

        return await SaveNewAsync(new FieldInspection(report.Id, report.PlotId, agronomistId, scheduledAt.ToUniversalTime()), ct);
    }

    public async Task<Result<FieldInspection>> CompleteAsync(CompleteCommand request, CancellationToken ct)
    {
        var (id, findings) = request;
        var inspection = await repository.FindInspectionAsync(id, ct);
        if (inspection is null) return Result<FieldInspection>.Failure(CropErrors.Missing("FieldInspection"));

        var now = time.GetUtcNow();
        if (inspection.Status != "SCHEDULED" || now < inspection.ScheduledAt)
            return Result<FieldInspection>.Failure(CropErrors.InspectionCannotComplete);

        var report = await repository.FindReportAsync(inspection.PestReportId, ct);
        inspection.Complete(findings, now);
        if (report?.Status == "PENDING")
            report.MarkInspected();

        return await SaveAsync(inspection, ct);
    }

    public async Task<Result<TechnicalPrescription>> IssueAsync(IssueCommand request, CancellationToken ct)
    {
        var (reportId, agronomistId, product, dosage, instructions, applicationDate) = request;
        var report = await repository.FindReportAsync(reportId, ct);
        if (report is null) return Result<TechnicalPrescription>.Failure(CropErrors.Missing("PestReport"));
        if (report.Status != "INSPECTED")
            return Result<TechnicalPrescription>.Failure(CropErrors.ReportMustBeInspected);

        var today = DateOnly.FromDateTime(TimeZoneInfo.ConvertTime(time.GetUtcNow(), BusinessZone).DateTime);
        if (applicationDate < today)
            return Result<TechnicalPrescription>.Failure(CropErrors.ApplicationDateInPast);

        return await SaveNewAsync(new TechnicalPrescription(report.Id, report.PlotId, agronomistId,
            product, dosage, instructions, applicationDate), ct);
    }

    public async Task<Result<TechnicalPrescription>> RegisterApplicationAsync(RegisterApplicationCommand request, CancellationToken ct)
    {
        var id = request.Id;
        var prescription = await repository.FindPrescriptionAsync(id, ct);
        if (prescription is null) return Result<TechnicalPrescription>.Failure(CropErrors.Missing("TechnicalPrescription"));
        if (prescription.Status == "APPLIED")
            return Result<TechnicalPrescription>.Failure(CropErrors.PrescriptionAlreadyApplied);

        prescription.RegisterApplication(time.GetUtcNow());
        return await SaveAsync(prescription, ct);
    }

    public Task<Result<RegionalBulletin>> PublishAsync(PublishCommand request, CancellationToken ct) =>
        SaveNewAsync(new RegionalBulletin(request.AgronomistId, request.Region, request.Title, request.Content, time.GetUtcNow()), ct);

    private async Task<Result<T>> SaveNewAsync<T>(T entity, CancellationToken ct) where T : class
    {
        repository.Add(entity);
        return await SaveAsync(entity, ct);
    }

    private async Task<Result<T>> SaveAsync<T>(T entity, CancellationToken ct) where T : class
    {
        try { await unitOfWork.CompleteAsync(ct); }
        catch (DbUpdateConcurrencyException)
        {
            return Result<T>.Failure(CropErrors.ConcurrentChange(typeof(T).Name));
        }
        catch (DbUpdateException exception) when (exception.InnerException is MySqlException { Number: 1062 })
        {
            return Result<T>.Failure(CropErrors.DuplicateForReport(typeof(T).Name));
        }
        return Result<T>.Success(entity);
    }

}
