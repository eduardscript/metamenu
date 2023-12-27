using Infra.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Presentation.Configuration;

public class JwtOptions(IOptions<JwtConfiguration> jwtConfiguration) : IConfigureNamedOptions<JwtBearerOptions>
{
    public void Configure(JwtBearerOptions options)
    {
        var jwtConfigurationValue = jwtConfiguration.Value;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtConfigurationValue.Issuer,
            ValidAudience = jwtConfigurationValue.Audience,
            IssuerSigningKey = jwtConfigurationValue.SignedKey,
            ValidAlgorithms = new[] { SecurityAlgorithms.HmacSha256 }
        };
    }

    public void Configure(string? name, JwtBearerOptions options)
    {
        Configure(options);
    }
}