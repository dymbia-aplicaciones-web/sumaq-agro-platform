using System.Text;
using SumaqAgro.Platform.Iam.Application.Internal.OutboundServices;

namespace SumaqAgro.Platform.Iam.Infrastructure.Hashing.BCrypt;

public sealed class BCryptHashingService : IHashingService
{
    private static readonly string DummyHash = HashPassword(Guid.NewGuid().ToString());

    public string Hash(string password) => HashPassword(password);

    public bool Verify(string password, string? hash) =>
        Encoding.UTF8.GetByteCount(password) <= 72 &&
        global::BCrypt.Net.BCrypt.Verify(password, hash ?? DummyHash);

    private static string HashPassword(string password) => global::BCrypt.Net.BCrypt.HashPassword(password);
}
