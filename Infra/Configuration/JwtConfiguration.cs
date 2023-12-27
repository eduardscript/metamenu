using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Infra.Configuration;

public class JwtConfiguration
{
    public string Key { get; set; } = default!;

    public string Issuer { get; set; } = default!;

    public string Audience { get; set; } = default!;
    
    public SymmetricSecurityKey SignedKey => new(Encoding.UTF8.GetBytes(Key));
}