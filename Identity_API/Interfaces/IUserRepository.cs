using Identity.API.Entities;
using Identity.API.Features.Users;
using Shared.Entities.Results;

namespace Identity.API.Interfaces;

internal interface IUserRepository
{
    Task<Result> Register(Register.Command command);
    Task<Result<LoginResponseDto>> Login(Login.Command command);
    Task<Result<string>> ForgotPassword(ForgotPassword.Command command);
    Task<Result> ResetPassword(ResetPassword.Command command);
}
