using SumaqAgro.Platform.Certification.Domain.Model.Commands;
using SumaqAgro.Platform.Certification.Domain.Model.Errors;
using SumaqAgro.Platform.Certification.Application.CommandServices;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using SumaqAgro.Platform.Certification.Domain.Model.Aggregates;
using SumaqAgro.Platform.Certification.Domain.Repositories;
using SumaqAgro.Platform.Shared.Application.Model;
using SumaqAgro.Platform.Shared.Domain.Model.Errors;
using SumaqAgro.Platform.Shared.Domain.Repositories;

namespace SumaqAgro.Platform.Certification.Application.Internal.CommandServices;

public sealed class CertificationService(ICertificationRepository repository, IUnitOfWork unitOfWork, TimeProvider time) : ICertificationService
{
    public async Task<Result<HarvestBatch>> RegisterAsync(RegisterHarvestBatchCommand command, CancellationToken ct)
    {
        var (code, cooperativeId, memberId, plotId, cropType, harvestedOn) = command;
        if (!await repository.MemberBelongsAsync(cooperativeId, memberId, ct))
            return Result<HarvestBatch>.Failure(CertificationErrors.Rule("The member does not belong to the cooperative."));

        var batch = new HarvestBatch(code, cooperativeId, memberId, plotId, cropType, harvestedOn, time.GetUtcNow());
        repository.Add(batch);
        return await SaveAsync(batch, ct);
    }

    public Task<Result<HarvestBatch>> WeighAsync(WeighHarvestBatchCommand command, CancellationToken ct) =>
        UpdateAsync(command.Id, batch => batch.Weigh(command.Gross, command.Tare), ct);

    public Task<Result<HarvestBatch>> SampleAsync(SampleHarvestBatchCommand command, CancellationToken ct) =>
        UpdateAsync(command.Id, batch => batch.Sample(command.Identifier, command.Weight), ct);

    public Task<Result<HarvestBatch>> GradeAsync(GradePotatoCalibersCommand command, CancellationToken ct) =>
        UpdateAsync(command.Id, batch => batch.GradePotato(command.Distribution), ct);

    public Task<Result<HarvestBatch>> CupAsync(CupCoffeeCommand command, CancellationToken ct) =>
        UpdateAsync(command.Id, batch => batch.CupCoffee(command.Score, command.FermentDefect), ct);

    private async Task<Result<HarvestBatch>> UpdateAsync(long id, Action<HarvestBatch> update, CancellationToken ct)
    {
        var batch = await repository.FindBatchAsync(id, ct);
        if (batch is null) return Result<HarvestBatch>.Failure(CertificationErrors.Missing("HarvestBatch"));
        try { update(batch); }
        catch (InvalidOperationException exception) { return Result<HarvestBatch>.Failure(CertificationErrors.Rule(exception.Message)); }
        return await SaveAsync(batch, ct);
    }

    public async Task<Result<QualityCertificate>> IssueAsync(IssueQualityCertificateCommand command, CancellationToken ct)
    {
        var (number, batchId, issuerId) = command;
        var batch = await repository.FindBatchAsync(batchId, ct);
        if (batch is null) return Result<QualityCertificate>.Failure(CertificationErrors.Missing("HarvestBatch"));
        if (batch.Status == "CERTIFIED")
            return Result<QualityCertificate>.Failure(CertificationErrors.BatchAlreadyCertified);
        try { batch.Certify(); }
        catch (InvalidOperationException exception) { return Result<QualityCertificate>.Failure(CertificationErrors.Rule(exception.Message)); }

        var issuedAt = time.GetUtcNow();
        issuedAt = issuedAt.AddTicks(-(issuedAt.Ticks % TimeSpan.TicksPerMillisecond));
        var canonical = FormattableString.Invariant(
            $"{number}|{batch.BatchCode}|{batch.Id}|{batch.CropType}|{batch.QualityCategory}|{batch.QualityScore}|{issuerId}|{issuedAt.UtcDateTime.ToString("yyyy-MM-dd'T'HH:mm:ss.FFF'Z'", CultureInfo.InvariantCulture)}");
        var hash = Convert.ToHexStringLower(SHA256.HashData(Encoding.UTF8.GetBytes(canonical)));
        var certificate = new QualityCertificate(number, batchId, batch.QualityCategory!, hash, issuerId, issuedAt);
        repository.Add(certificate);
        return await SaveAsync(certificate, ct);
    }

    public async Task<Result<QualityCertificate>> RevokeAsync(RevokeQualityCertificateCommand command, CancellationToken ct)
    {
        var id = command.Id;
        var certificate = await repository.FindCertificateAsync(id, ct);
        if (certificate is null) return Result<QualityCertificate>.Failure(CertificationErrors.Missing("QualityCertificate"));
        try { certificate.Revoke(); }
        catch (InvalidOperationException exception) { return Result<QualityCertificate>.Failure(CertificationErrors.Rule(exception.Message)); }
        return await SaveAsync(certificate, ct);
    }

    private async Task<Result<T>> SaveAsync<T>(T entity, CancellationToken ct) where T : class
    {
        try { await unitOfWork.CompleteAsync(ct); }
        catch (DbUpdateConcurrencyException)
        {
            return Result<T>.Failure(CertificationErrors.ConcurrentChange(typeof(T).Name));
        }
        catch (DbUpdateException exception) when (exception.InnerException is MySqlException { Number: 1062 })
        {
            return Result<T>.Failure(CertificationErrors.Duplicate(typeof(T).Name));
        }
        return Result<T>.Success(entity);
    }

}
