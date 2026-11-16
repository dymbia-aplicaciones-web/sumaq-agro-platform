namespace SumaqAgro.Platform.Iam.Interfaces.Acl;

public interface IIamAccounts
{
    Task<IamAccount?> FindAsync(string email, CancellationToken ct);
}

public sealed record IamAccount(long Id, string Role);
