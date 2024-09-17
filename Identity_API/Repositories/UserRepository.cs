﻿using System.Security.Claims;
using BaseDomain.Results;
using Identity.API.Databases;
using Identity.API.Domains.Tokens;
using Identity.API.Domains.Users;
using Identity.API.Errors;
using Identity.API.Features.Users;
using Identity.API.Interfaces;
using Microsoft.AspNetCore.Identity;
using Shared.Extensions.JwtHandlers;

namespace Identity.API.Repositories;

internal class UserRepository(
    UserManager<User> userManager,
    SignInManager<User> signInManager,
    UserDbContext dbContext,
    JwtHandler jwtHandler
) : IUserRepository
{
    public async Task<Result> Register(User user, string password)
    {
        var result = await userManager.CreateAsync(user, password);

        return !result.Succeeded ? Result.Failure(UserErrors.WentWrong) : Result.Success();
    }

    public async Task<Result<LoginResponse>> Login(Login.Command command)
    {
        var user = await userManager.FindByEmailAsync(command.Email!);
        if (user is null)
            return Result.Failure<LoginResponse>(UserErrors.NotFound);

        var result = await signInManager.PasswordSignInAsync(
            user.UserName!,
            command.Password!,
            true,
            false
        );
        if (!result.Succeeded)
            return Result.Failure<LoginResponse>(UserErrors.NotFound);

        List<Claim> claims =
        [
            new("Username", user.UserName!),
            new("UserId", user.Id.ToString()),
            new("Email", user.Email!),
            new("Avatar", user.EmailConfirmed.ToString()),
        ];

        var accessToken = jwtHandler.GenerateAccessToken(claims);
        var refreshToken = jwtHandler.GenerateRefreshToken();
        var expiredTime = DateTime.Now.AddDays(1);
        if (user.RefreshToken is null)
        {
            var token = RefreshToken.Create(refreshToken, expiredTime);
            user.AddToken(token);
        }

        user.RefreshToken!.Refresh(refreshToken, expiredTime);
        await dbContext.SaveChangesAsync();

        var loginResponse = new LoginResponse(refreshToken, accessToken);
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
