using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using SumaqAgro.Platform.Iam.Application.CommandServices;
using SumaqAgro.Platform.Iam.Application.Internal.OutboundServices;
using SumaqAgro.Platform.Iam.Domain.Model.Aggregates;
using SumaqAgro.Platform.Iam.Domain.Model.ValueObjects;
using SumaqAgro.Platform.Iam.Domain.Repositories;
using SumaqAgro.Platform.Shared.Domain.Repositories;

namespace SumaqAgro.Platform.Iam.Application.Internal.CommandServices;

public sealed class PasswordRecoveryService(IUserRepository users, IPasswordResetRepository tokens,
    IPasswordResetEmail email, IHashingService hashing, IUnitOfWork work, TimeProvider clock,
    ILogger<PasswordRecoveryService> logger) : IPasswordRecoveryService
{
    public async Task RequestAsync(string address, CancellationToken ct)
    {
        var user = await users.FindByEmailAsync(new EmailAddress(address), ct);
        if (user is null) return;

        var secret = WebEncoders.Base64UrlEncode(RandomNumberGenerator.GetBytes(32));
        tokens.Add(new PasswordResetToken(Digest(secret), user.Id, clock.GetUtcNow().UtcDateTime.AddMinutes(15)));
        await work.CompleteAsync(ct);

        try { await email.SendAsync(user.Email, secret, ct); }
        catch (HttpRequestException)
        {
            logger.LogWarning("The password recovery email provider could not deliver a message.");
        }
        catch (OperationCanceledException) when (!ct.IsCancellationRequested)
        {
            logger.LogWarning("The password recovery email provider timed out.");
        }
    }

    public async Task<bool> ResetAsync(string secret, string password, CancellationToken ct)
    {
        if (Encoding.UTF8.GetByteCount(password) > 72) return false;
        var token = await tokens.FindAsync(Digest(secret), ct);
        if (token is null || !token.TryConsume(clock.GetUtcNow().UtcDateTime)) return false;

        var user = await tokens.FindUserAsync(token.UserId, ct);
        if (user is null) return false;

        user.ChangePassword(hashing.Hash(password));
        try { await work.CompleteAsync(ct); }
        catch (DbUpdateConcurrencyException) { return false; }

        return true;
    }

    private static string Digest(string token) => Convert.ToHexStringLower(SHA256.HashData(Encoding.UTF8.GetBytes(token)));
}
