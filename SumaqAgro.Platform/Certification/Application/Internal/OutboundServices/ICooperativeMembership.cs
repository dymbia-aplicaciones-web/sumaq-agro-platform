namespace SumaqAgro.Platform.Certification.Application.Internal.OutboundServices;

public interface ICooperativeMembership
{
    Task<bool> MemberBelongsAsync(long cooperativeId, long memberId, CancellationToken ct);
}
