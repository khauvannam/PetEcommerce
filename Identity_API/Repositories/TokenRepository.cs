using System.Security.Claims;
using Base.Results;
using Identity.API.Databases;
using Identity.API.Domains.Users;
using Identity.API.DTOs.Tokens;
using Identity.API.Errors;
using Identity.API.Features.Tokens;
using Identity.API.Interfaces;
using Microsoft.EntityFrameworkCore;
using Share.Extensions.JwtHandlers;

namespace Identity.API.Repositories;

public class TokenRepository(UserDbContext dbContext) : ITokenRepository
{
    public async Task<Result<TokenResponse>> Refresh(Refresh.Command command)
    {
        var claimsPrincipal = JwtHandler.GetClaimsPrincipalFromExpiredToken(command.AccessToken);
        var userIdString = claimsPrincipal.FindFirstValue("UserId");

        if (!int.TryParse(userIdString, out var userId))
            return Result.Failure<TokenResponse>(UserErrors.NotFound);

        var user = dbContext.Users.Include(u => u.RefreshToken).FirstOrDefault(u => u.Id == userId);

        if (user is null)
            return Result.Failure<TokenResponse>(UserErrors.NotFound);

        if (CheckTokenIsExpired(user))
            return Result.Failure<TokenResponse>(TokenErrors.ExpiredToken());

        var refreshToken = JwtHandler.GenerateRefreshToken();
        var accessToken = JwtHandler.GenerateAccessToken(claimsPrincipal.Claims);
        var expiredTime = DateTime.Now.AddMonths(1);
        var tokenResponseDto = new TokenResponse(accessToken);

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

    private bool CheckTokenIsExpired(User user)
    {
        return user.RefreshToken!.ExpiredAt <= DateTime.Now;
    }
}
