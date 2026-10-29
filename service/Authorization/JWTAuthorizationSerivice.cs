using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;


namespace be.Authorization;

public class JWTAuthorizationSerivice(IConfiguration configuration) : IAuthorizationService
{
    private const string Issuer = "sec-api-jwt";
    private const string Audience = "sec-api-jwt";
    private readonly SymmetricSecurityKey _key = new(Encoding.UTF8.GetBytes(
        configuration["JWT:Secret"] ?? throw new InvalidOperationException("JWT:Secret is missing.")));
    public string GenerateJwtToken(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.id.ToString()),
            new Claim(ClaimTypes.Name, user.username)
        };

        var token = new JwtSecurityToken(
            issuer: Issuer,
            audience: Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(30),
            signingCredentials: new SigningCredentials(_key, SecurityAlgorithms.HmacSha256)
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
