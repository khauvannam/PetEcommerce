using System.Security.Claims;
using BaseDomain.Results;
using Identity.API.Databases;
using Identity.API.Domains.Tokens;
using Identity.API.Domains.Users;
using Identity.API.Errors;
using Identity.API.Features.Tokens;
using Identity.API.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Extensions.JwtHandlers;

namespace Identity.API.Repositories;

public class TokenRepository(UserDbContext dbContext, JwtHandler jwtHandler) : ITokenRepository
{
    public async Task<Result<TokenResponse>> Refresh(Refresh.Command command)
    {
        var claimsPrincipal = jwtHandler.GetClaimsPrincipalFromExpiredToken(command.AccessToken);
        var userId = claimsPrincipal.FindFirstValue("UserId");
        var user = dbContext
            .Users.Include(user => user.RefreshToken)
            .FirstOrDefault(u => u.Id == userId);
        if (user is null)
            return Result.Failure<TokenResponse>(UserErrors.NotFound);

        if (CheckInValidToken(user, command.RefreshToken))
            return Result.Failure<TokenResponse>(TokenErrors.WrongToken("refresh token"));

        if (ChecKTokenIsExpired(user))
            return Result.Failure<TokenResponse>(TokenErrors.ExpiredToken());

        var refreshToken = jwtHandler.GenerateRefreshToken();
        var accessToken = jwtHandler.GenerateAccessToken(claimsPrincipal.Claims);
        var expiredTime = DateTime.Now.AddMonths(1);
        var tokenResponseDto = new TokenResponse(refreshToken, accessToken);

        user.RefreshToken!.Refresh(refreshToken, expiredTime);
        await dbContext.SaveChangesAsync();
        return Result.Success(tokenResponseDto);
    }

    public async Task<Result> Revoke(Revoke.Command command)
    {
        var userId = command.UserId;
        var user = dbContext
            .Users.Include(user => user.RefreshToken!)
            .FirstOrDefault(u => u.Id == userId);
        if (user is null)
            return Result.Failure(UserErrors.NotFound);

        dbContext.RefreshTokens.Remove(user.RefreshToken!);
        user.RevokeToken();
        await dbContext.SaveChangesAsync();
        return Result.Success();
    }

    private bool CheckInValidToken(User user, string refreshToken)
    {
        return user.RefreshToken!.Token != refreshToken;
    }

    private bool ChecKTokenIsExpired(User user)
    {
        return user.RefreshToken!.ExpiredAt <= DateTime.Now;
    }
}
