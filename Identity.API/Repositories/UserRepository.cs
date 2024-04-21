using Identity.API.Databases;
using Identity.API.Entities;
using Identity.API.Errors;
using Identity.API.Features.Users;
using Identity.API.Interfaces;
using Microsoft.AspNetCore.Identity;
using MimeKit;
using Shared.Extensions.JwtHandlers;
using Shared.Services;
using Shared.Shared;

namespace Identity.API.Repositories;

internal class UserRepository(
    UserManager<User> userManager,
    SignInManager<User> signInManager,
    UserDbContext dbContext,
    JwtHandler jwtHandler
) : IUserRepository
{
    public async Task<Result> Register(Register.Command command)
    {
        var user = new User
        {
            Email = command.Email,
            UserName = command.Username,
            SecurityStamp = command.SecurityStamp,
            PhoneNumber = command.PhoneNumber
        };

        var result = await userManager.CreateAsync(user, command.Password!);
        if (result.Succeeded)
        {
            return Result.Failure(UserErrors.WentWrong);
        }

        return Result.Success();
    }

    public async Task<Result<LoginResponseDto>> Login(Login.Command command)
    {
        var user = await userManager.FindByEmailAsync(command.Email!);
        if (user is null)
        {
            return Result.Failure<LoginResponseDto>(UserErrors.NotFound);
        }

        var result = await signInManager.PasswordSignInAsync(
            user.UserName!,
            command.Password!,
            true,
            false
        );
        if (!result.Succeeded)
        {
            return Result.Failure<LoginResponseDto>(UserErrors.NotFound);
        }

        var claims = await userManager.GetClaimsAsync(user);
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

        var loginResponse = new LoginResponseDto(refreshToken, accessToken);
        return Result.Success(loginResponse);
    }

    public async Task<Result<string>> ForgotPassword(ForgotPassword.Command command)
    {
        var user = await userManager.FindByEmailAsync(command.Email!);
        if (user is null)
        {
            return Result.Failure<string>(UserErrors.NotFound);
        }
        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        var message = EmailService.CreateEmail(
            command.Email!,
            user.UserName!,
            "Your Reset Password Code",
            new TextPart("plain") { Text = token }
        );
        await EmailService.SendEmailAsync(message);
        return Result.Success(user.Email!);
    }

    public async Task<Result> ResetPassword(ResetPassword.Command command)
    {
        var user = await userManager.FindByEmailAsync(command.Email!);
        if (user is null)
        {
            return Result.Failure(UserErrors.NotFound);
        }

        var resultPasswordAsync = await userManager.ResetPasswordAsync(
            user,
            command.Token,
            command.Password
        );
        if (!resultPasswordAsync.Succeeded)
        {
            return Result.Failure(TokenErrors.WrongToken("Reset Password"));
        }
        return Result.Success();
    }
}
