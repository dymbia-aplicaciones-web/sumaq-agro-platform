namespace SumaqAgro.Platform.Certification.Domain.Model.Commands;

public sealed record IssueQualityCertificateCommand(string Number, long BatchId, long IssuerId);