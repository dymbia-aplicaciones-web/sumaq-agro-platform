using SumaqAgro.Platform.Certification.Domain.Model.ValueObjects;

namespace SumaqAgro.Platform.Certification.Domain.Model.Commands;

public sealed record GradePotatoCalibersCommand(long Id, PotatoCaliberDistribution Distribution);