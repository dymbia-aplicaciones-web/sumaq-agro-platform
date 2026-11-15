using SumaqAgro.Platform.Profiles.Interfaces.Acl;
using SumaqAgro.Platform.Certification.Application.Internal.OutboundServices;

namespace SumaqAgro.Platform.Certification.Infrastructure.Acl;

public sealed class CooperativeMembership(IProfilesContextFacade profiles) : ICooperativeMembership
{
    public Task<bool> MemberBelongsAsync(long cooperativeId, long memberId, CancellationToken ct) =>
        profiles.MemberBelongsAsync(cooperativeId, memberId, ct);
}
