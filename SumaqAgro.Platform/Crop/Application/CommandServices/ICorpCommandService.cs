using SumaqAgro.Platform.Crop.Domain.Model.Commands;
using SumaqAgro.Platform.Crop.Domain.Model.Aggregates;
using SumaqAgro.Platform.Shared.Application.Model;
using SumaqAgro.Platform.Shared.Domain.Model.Errors;

namespace SumaqAgro.Platform.Crop.Application.CommandServices;

public interface ICropCommandService
{
    Task<Result<PestReport>> ReportAsync(ReportCommand request, CancellationToken ct);

    Task<Result<string>> DeleteReportAsync(DeleteReportCommand request, CancellationToken ct);

    Task<Result<FieldInspection>> ScheduleAsync(ScheduleCommand request, CancellationToken ct);

    Task<Result<FieldInspection>> CompleteAsync(CompleteCommand request, CancellationToken ct);

    Task<Result<TechnicalPrescription>> IssueAsync(IssueCommand request, CancellationToken ct);

    Task<Result<TechnicalPrescription>> RegisterApplicationAsync(RegisterApplicationCommand request, CancellationToken ct);

    Task<Result<RegionalBulletin>> PublishAsync(PublishCommand request, CancellationToken ct);
}
