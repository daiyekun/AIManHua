using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AIManHua.Infrastructure.Services;

public class JwtService
{
    private readonly string _issuer;
    private readonly string _audience;
    private readonly SymmetricSecurityKey _key;
    private readonly int _expirationMinutes;

    public JwtService(IConfiguration configuration)
    {
        var jwtSection = configuration.GetSection("Jwt");
        _issuer = jwtSection["Issuer"] ?? "AIManHua";
        _audience = jwtSection["Audience"] ?? "AIManHua";
        var secret = jwtSection["Secret"]
            ?? throw new InvalidOperationException("JWT Secret is not configured");
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        _expirationMinutes = int.TryParse(jwtSection["ExpirationMinutes"], out var minutes) ? minutes : 1440;
    }

    public (string Token, DateTime ExpiresAt) GenerateToken(long userId, string email, string username)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Name, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var expiresAt = DateTime.UtcNow.AddMinutes(_expirationMinutes);
        var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            expires: expiresAt,
            signingCredentials: credentials
        );

        return (new JwtSecurityTokenHandler().WriteToken(token), expiresAt);
    }
}
