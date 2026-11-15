using SumaqAgro.Platform.Iam.Interfaces.Acl;
using SumaqAgro.Platform.Crop.Application.Internal.OutboundServices;

namespace SumaqAgro.Platform.Crop.Infrastructure.Acl;

public sealed class CropAccounts(IIamAccounts iam) : ICropAccounts
{
    public async Task<long?> FindIdAsync(string email, CancellationToken ct) =>
        (await iam.FindAsync(email, ct))?.Id;
}
