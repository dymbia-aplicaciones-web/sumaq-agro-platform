namespace SumaqAgro.Platform.Iam.Domain.Model.Aggregates;

public sealed class PasswordResetToken(string digest, long userId, DateTime expiresAt)
{
    public string Digest { get; private set; } = digest;
    public long UserId { get; private set; } = userId;
    public DateTime ExpiresAt { get; private set; } = expiresAt;
    public bool Used { get; private set; }

    public bool TryConsume(DateTime now)
    {
        if (Used || now >= ExpiresAt) return false;
        Used = true;
        return true;
    }
}
