using System.Security.Claims;
using Base.Results;
using Identity.API.Databases;
using Identity.API.Domains.Tokens;
using Identity.API.Domains.Users;
using Identity.API.DTOs.Users;
using Identity.API.Errors;
using Identity.API.Features.Users;
using Identity.API.Interfaces;
using Microsoft.AspNetCore.Identity;
using Share.Extensions.JwtHandlers;

namespace Identity.API.Repositories;

internal class UserRepository(
    UserManager<User> userManager,
    SignInManager<User> signInManager,
    UserDbContext dbContext
) : IUserRepository
{
    public async Task<Result> Register(User user, string password)
    {
        if (await userManager.CreateAsync(user, password) is { Succeeded: false } result)
        {
            var errorMessages = string.Join(", ", result.Errors.Select(error => error.Description));
            return Result.Failure(new("Invalid Request:", errorMessages));
        }

        return Result.Success();
    }

    public async Task<Result<LoginResponse>> Login(Login.Command command)
    {
        var user = await userManager.FindByEmailAsync(command.Email);

        if (user is null)
            return Result.Failure<LoginResponse>(UserErrors.NotFound);

        await dbContext.Entry(user).Reference(u => u.RefreshToken).LoadAsync();

        var result = await signInManager.PasswordSignInAsync(
            user.UserName!,
            command.Password,
            true,
            false
        );
        if (!result.Succeeded)
            return Result.Failure<LoginResponse>(UserErrors.WrongPassword);

        List<Claim> claims =
        [
            new("Username", user.UserName!),
            new("UserId", user.Id.ToString()),
            new("Email", user.Email!),
        ];

        var accessToken = JwtHandler.GenerateAccessToken(claims);
        var refreshToken = JwtHandler.GenerateRefreshToken();
        var expiredTime = DateTime.Now.AddMonths(1);

        if (user.RefreshToken is not null)
        {
            user.RefreshToken.Refresh(refreshToken, expiredTime);
        }
        else
        {
            var token = RefreshToken.Create(refreshToken, expiredTime);
            user.AddToken(token);
        }

        await dbContext.SaveChangesAsync();

        var loginResponse = new LoginResponse(accessToken);

        return Result.Success(loginResponse);
    }

    public async Task<Result> ResetPassword(ResetPassword.Command command)
    {
        var user = await userManager.FindByEmailAsync(command.Email);
        if (user is null)
            return Result.Failure(UserErrors.NotFound);

        var resultPasswordAsync = await userManager.ResetPasswordAsync(
            user,
            command.Token,
            command.Password
        );

        if (!resultPasswordAsync.Succeeded)
            return Result.Failure(TokenErrors.WrongToken("Reset Password"));

        return Result.Success();
    }
}
