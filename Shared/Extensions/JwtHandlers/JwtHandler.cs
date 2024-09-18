using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Shared.Domain.Services;

namespace Shared.Extensions.JwtHandlers;

public class JwtHandler
{
    private readonly string _secret = Key.JwtSecret;

    private SigningCredentials GetSigningCredentials()
    {
        var key = Encoding.UTF8.GetBytes(_secret);
        var secret = new SymmetricSecurityKey(key);
        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }

    public string GenerateAccessToken(IEnumerable<Claim> claims)
    {
        var signingCredentials = GetSigningCredentials();
        var tokenOption = new JwtSecurityToken(
            "http://localhost:8080",
            "http://localhost:8080",
            claims,
            expires: DateTime.Now.AddDays(7),
            signingCredentials: signingCredentials
        );
        var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOption);
        return tokenString;
    }

    public static string GenerateRefreshToken()
    {
        var refreshToken = Guid.NewGuid().ToString();
        return refreshToken;
    }

    public ClaimsPrincipal GetClaimsPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = false,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret)),
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var principals = tokenHandler.ValidateToken(
            token,
            tokenValidationParameters,
            out var securityToken
        );
        if (securityToken is null)
            throw new SecurityTokenException("Invalid Token");

        return principals;
    }
}
