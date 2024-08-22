using BaseDomain.Results;
using Identity.API.Domains.Users;
using Identity.API.Errors;
using Microsoft.AspNetCore.Identity;
using MimeKit;
using Shared.Services;

namespace Identity.API.Services;

public class UserEmailService(UserManager<User> userManager)
{
    public async Task<Result<string>> SendResetPasswordEmail(string email)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null)
        {
            return Result.Failure<string>(UserErrors.NotFound);
        }

        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        var message = EmailService.CreateEmail(
            email,
            user.UserName!,
            "Your Reset Password Code",
            new TextPart("plain") { Text = token }
        );
        await EmailService.SendEmailAsync(message);
        return Result.Success(user.Email!);
    }
}