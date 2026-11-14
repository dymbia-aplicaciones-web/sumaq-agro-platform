using System.ComponentModel.DataAnnotations;

namespace SumaqAgro.Platform.Certification.Interfaces.Rest.Resources;

public sealed record IssueCertificate([param: Required, StringLength(40)] string CertificateNumber,
    [param: Range(1, long.MaxValue)] long HarvestBatchId, [param: Range(1, long.MaxValue)] long IssuedByUserId);