using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Share.Domain.Services;

namespace Share.Extensions.JwtHandlers;

public static class JwtHandler
{
    private const string Secret = Key.JwtSecret;

    private static SigningCredentials GetSigningCredentials()
    {
        var key = Encoding.UTF8.GetBytes(Secret);
        var secret = new SymmetricSecurityKey(key);
        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256Signature);
    }

    public static string GenerateAccessToken(IEnumerable<Claim> claims)
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

    public static ClaimsPrincipal GetClaimsPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = false,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret)),
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
