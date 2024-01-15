using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Core.Entities;
using Core.Services;
using Infra.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infra.Services;

public class TokenService(IOptions<JwtConfiguration> jwtConfiguration) : ITokenService
{
    private readonly JwtConfiguration _jwtConfiguration = jwtConfiguration.Value;

    public Task<string> GenerateTokenAsync(User user, CancellationToken cancellationToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        
        var now = DateTime.UtcNow;
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
            }),
            Expires = now.AddMinutes(5),
            SigningCredentials = new SigningCredentials(_jwtConfiguration.SignedKey, SecurityAlgorithms.HmacSha256Signature),
            Issuer = _jwtConfiguration.Issuer,
            Audience = _jwtConfiguration.Audience,
            IssuedAt = now,
        };
        
        var token = tokenHandler.CreateToken(tokenDescriptor);
        
        return Task.FromResult(tokenHandler.WriteToken(token));
    }
}