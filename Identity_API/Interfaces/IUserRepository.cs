using Identity.API.Domain.Users;
using Identity.API.Features.Users;
using Shared.Domain.Results;

namespace Identity.API.Interfaces;

internal interface IUserRepository
{
    Task<Result> Register(Register.Command command);
    Task<Result<LoginResponse>> Login(Login.Command command);
    Task<Result<string>> ForgotPassword(ForgotPassword.Command command);
    Task<Result> ResetPassword(ResetPassword.Command command);
}
