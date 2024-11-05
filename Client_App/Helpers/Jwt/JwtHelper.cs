using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Client_App.DTOs.Share;
using Client_App.Helpers.Secrets;
using Microsoft.IdentityModel.Tokens;

namespace Client_App.Helpers.Jwt;

public class JwtHelper
{
    private const string Secret = Key.JwtSecret;

    public static Result<ClaimsPrincipal> GetClaimsPrincipalFromToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = false,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret)),
            ClockSkew = TimeSpan.Zero,
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var principals = tokenHandler.ValidateToken(
            token,
            tokenValidationParameters,
            out var securityToken
        );

        if (
            securityToken is not JwtSecurityToken jwtSecurityToken
            || !jwtSecurityToken.Header.Alg.Equals(
                SecurityAlgorithms.HmacSha256Signature,
                StringComparison.InvariantCultureIgnoreCase
            )
        )
            return Result.Failure<ClaimsPrincipal>(
                new ErrorType("Invalid token", "You need to provide a valid JWT token")
            );

        return Result.Success(principals);
    }

    public static bool CheckExpiredToken(string token)
    {
        var claimsPrincipal = GetClaimsPrincipalFromToken(token);

        var expClaim = claimsPrincipal.Value.FindFirst("exp")?.Value;

        if (expClaim is not null && long.TryParse(expClaim, out var expSeconds))
        {
            var tokenExpiryDate = DateTimeOffset.FromUnixTimeSeconds(expSeconds).UtcDateTime;

            return tokenExpiryDate <= DateTime.Now;
        }

        // If the expiration claim is missing or invalid, consider the token as expired
        return true;
    }
}
