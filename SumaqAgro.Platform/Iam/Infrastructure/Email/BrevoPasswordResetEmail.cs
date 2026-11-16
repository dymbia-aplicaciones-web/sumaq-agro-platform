using System.Net.Http.Json;
using Microsoft.AspNetCore.WebUtilities;
using SumaqAgro.Platform.Iam.Application.Internal.OutboundServices;

namespace SumaqAgro.Platform.Iam.Infrastructure.Email;

public sealed class BrevoPasswordResetEmail(HttpClient client, IConfiguration configuration) : IPasswordResetEmail
{
    public async Task SendAsync(string email, string token, CancellationToken ct)
    {
        var apiKey = configuration["Iam:PasswordRecovery:Brevo:ApiKey"];
        var sender = configuration["Iam:PasswordRecovery:Brevo:SenderEmail"];
        var resetUrl = configuration["Iam:PasswordRecovery:ResetUrl"];
        if (string.IsNullOrWhiteSpace(apiKey) || string.IsNullOrWhiteSpace(sender) ||
            !Uri.TryCreate(resetUrl, UriKind.Absolute, out var resetUri) || resetUri.Scheme != Uri.UriSchemeHttps)
            throw new HttpRequestException("Password recovery email is not configured.");

        using var request = new HttpRequestMessage(HttpMethod.Post, "https://api.brevo.com/v3/smtp/email");
        request.Headers.Add("api-key", apiKey);
        request.Content = JsonContent.Create(new
        {
            sender = new { email = sender, name = "SumaqAgro" },
            to = new[] { new { email } },
            subject = "Reset your SumaqAgro password",
            textContent = $"Reset your password within 15 minutes: {QueryHelpers.AddQueryString(resetUrl!, "token", token)}"
        });
        using var response = await client.SendAsync(request, ct);
        response.EnsureSuccessStatusCode();
    }
}
