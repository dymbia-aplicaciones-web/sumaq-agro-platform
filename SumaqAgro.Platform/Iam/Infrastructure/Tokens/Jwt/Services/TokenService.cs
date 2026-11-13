using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using SumaqAgro.Platform.Iam.Application.Internal.OutboundServices;

namespace SumaqAgro.Platform.Iam.Infrastructure.Tokens.Jwt;

public sealed class TokenService(SymmetricSecurityKey key, int expirationMinutes) : ITokenService
{
    public string Generate(string email)
    {
        var now = DateTime.UtcNow;
        var token = new JwtSecurityToken(
            claims: [new Claim(JwtRegisteredClaimNames.Sub, email)],
            notBefore: now,
            expires: now.AddMinutes(expirationMinutes),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));
        token.Payload[JwtRegisteredClaimNames.Iat] = EpochTime.GetIntDate(now);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
