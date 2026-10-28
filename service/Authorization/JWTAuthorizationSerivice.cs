using System.Text;
using Microsoft.Extensions.Configuration;

namespace be.Authorization;

public class JWTAuthorizationSerivice(IConfiguration configuration) : IAuthorizationService
{
    private const string Issuer = "sec-api-jwt";
    private const string Audience = "sec-api-jwt";
    private readonly SymmetricSecurityKey _key = new(Encoding.UTF8.GetBytes(configuration["JWT:Secret"] ?? throw new InvalidOperationException("JwtSecret is missing")));
    public string GenerateJwtToken(User user)
    {
        throw new NotImplementedException();
    }
}